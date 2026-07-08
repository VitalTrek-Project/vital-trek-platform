using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.OutboundServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Transform;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest;

/// <summary>
/// Subscription + Stripe Checkout endpoints. Checkout, status and cancel require an
/// authenticated user (the subscribed user is always the caller from the JWT, never a path
/// parameter). The webhook is the only public endpoint here — Stripe calls it directly and
/// authenticates via the signed payload, not a session/JWT.
/// TODO: real production plans would validate role (e.g. Tourist-only) — skipped for the demo.
/// </summary>
[ApiController]
[Route("api/v1/subscriptions")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available subscription & payment endpoints")]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService,
    IPaymentGatewayService paymentGatewayService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("checkout")]
    [Authorize]
    [SwaggerOperation(Summary = "Start a Stripe Checkout session for the current user", OperationId = "CreateCheckoutSession")]
    [SwaggerResponse(StatusCodes.Status200OK, "The checkout URL was created", typeof(CheckoutSessionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid plan")]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "Stripe is not configured yet")]
    public async Task<IActionResult> CreateCheckoutSession(
        [FromBody] CreateCheckoutSessionResource resource, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<SubscriptionPlan>(resource.Plan, true, out var plan))
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status400BadRequest, SubscriptionsError.InvalidPlan,
                $"'{resource.Plan}' is not a valid plan. Expected 'Monthly' or 'Annual'.");

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await commandService.Handle(new CreateCheckoutSessionCommand(userId, plan), cancellationToken);

        return SubscriptionsActionResultAssembler.ToActionResultFromResult(
            this, result, errorLocalizer, problemDetailsFactory,
            checkoutUrl => Ok(new CheckoutSessionResource(checkoutUrl)));
    }

    [HttpGet("me")]
    [Authorize]
    [SwaggerOperation(Summary = "Get the current user's subscription", OperationId = "GetMySubscription")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription was found", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No subscription exists for this user")]
    public async Task<IActionResult> GetMySubscription(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var subscription = await queryService.Handle(new GetSubscriptionByUserIdQuery(userId), cancellationToken);

        if (subscription is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, SubscriptionsError.SubscriptionNotFound,
                "No subscription exists for this user.");

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpPost("me/cancel")]
    [Authorize]
    [SwaggerOperation(Summary = "Cancel the current user's active subscription", OperationId = "CancelMySubscription")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The subscription was canceled")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No active subscription to cancel")]
    public async Task<IActionResult> CancelMySubscription(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await commandService.Handle(new CancelSubscriptionCommand(userId), cancellationToken);

        return SubscriptionsActionResultAssembler.ToActionResultFromResult(
            this, result, errorLocalizer, problemDetailsFactory, () => NoContent());
    }

    /// <summary>
    /// Stripe webhook receiver. Public by design — Stripe has no session/JWT, it authenticates
    /// via the "Stripe-Signature" header verified against Stripe:WebhookSecret.
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Stripe webhook receiver", OperationId = "StripeWebhook")]
    [SwaggerResponse(StatusCodes.Status200OK, "The event was processed")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid signature or payload")]
    public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        PaymentWebhookEvent stripeEvent;
        try
        {
            stripeEvent = paymentGatewayService.ParseWebhookEvent(payload, signature);
        }
        catch (PaymentGatewayNotConfiguredException ex)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status503ServiceUnavailable, SubscriptionsError.StripeNotConfigured, ex.Message);
        }
        catch (Exception)
        {
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status400BadRequest, SubscriptionsError.StripeError, "Invalid Stripe webhook signature or payload.");
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed" when stripeEvent.CheckoutSessionId is not null:
                var start = DateTimeOffset.UtcNow;
                // TODO: derive the real plan-based end date from the Subscription's Plan once fetched;
                // Stripe's own subscription renewal is the source of truth, this is only a local mirror.
                await commandService.Handle(
                    new ActivateSubscriptionCommand(stripeEvent.CheckoutSessionId, stripeEvent.StripeSubscriptionId, start, start.AddMonths(1)),
                    cancellationToken);
                break;
            // TODO: only covers a failure tied to the original checkout session; a failure on a
            // later renewal cycle carries a subscription id, not a checkout session id, and needs
            // a FindByStripeSubscriptionIdAsync lookup we haven't built yet — out of scope for today.
            case "invoice.payment_failed" when stripeEvent.CheckoutSessionId is not null:
                await commandService.Handle(new MarkPaymentFailedCommand(stripeEvent.CheckoutSessionId), cancellationToken);
                break;
        }

        return Ok();
    }
}
