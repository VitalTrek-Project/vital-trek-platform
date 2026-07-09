using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Mock.Configuration;
using NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Transform;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest;

/// <summary>
/// Subscription + mock-payment-gateway endpoints. Checkout, status and cancel require an
/// authenticated user (the subscribed user is always the caller from the JWT, never a path
/// parameter). The mock-checkout pages are the only public endpoints here — they stand in for
/// a real gateway's hosted checkout page + webhook, so there is no session/JWT to check there.
/// TODO: real production plans would validate role (e.g. Tourist-only) — skipped for the demo.
/// TODO: swap the mock gateway for a real one (Stripe, etc.) via the Program.cs DI registration
/// when there's time/credentials — IPaymentGatewayService callers here don't need to change.
/// </summary>
[ApiController]
[Route("api/v1/subscriptions")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available subscription & payment endpoints")]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService,
    IOptions<PaymentGatewaySettings> paymentSettings,
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

    /// <summary>
    /// Updates the current user's subscription. Replaces the old verb-suffixed
    /// <c>POST me/cancel</c> action route with a state-change PATCH on the subscription
    /// resource itself. Only Status "Canceled" is a supported transition today.
    /// </summary>
    [HttpPatch("me")]
    [Authorize]
    [SwaggerOperation(Summary = "Update the current user's subscription (cancel it)", OperationId = "UpdateMySubscription")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The subscription was updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid status")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No active subscription to cancel")]
    public async Task<IActionResult> UpdateMySubscription(
        [FromBody] UpdateSubscriptionResource resource, CancellationToken cancellationToken)
    {
        if (!string.Equals(resource.Status, "Canceled", StringComparison.OrdinalIgnoreCase))
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status400BadRequest, (Enum?)null,
                $"'{resource.Status}' is not a supported status. Expected 'Canceled'.");

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await commandService.Handle(new CancelSubscriptionCommand(userId), cancellationToken);

        return SubscriptionsActionResultAssembler.ToActionResultFromResult(
            this, result, errorLocalizer, problemDetailsFactory, () => NoContent());
    }

    /// <summary>
    /// Mock hosted checkout page — stands in for a real gateway's Checkout page (e.g. Stripe
    /// Checkout). Public by design: this is the page the "customer" lands on after
    /// <see cref="CreateCheckoutSession" /> returns its URL, before they have a session there.
    /// TODO: delete this whole mock-checkout trio once a real gateway is wired in; a real
    /// provider hosts its own page and calls our webhook instead.
    /// </summary>
    [HttpGet("mock-checkout/{sessionId}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Mock hosted checkout page (stand-in for a real gateway)", OperationId = "MockCheckoutPage")]
    public ContentResult MockCheckoutPage(string sessionId)
    {
        // Native <form> only supports GET/POST, so a PATCH here needs fetch() + JS redirect
        // instead of a form submission — the endpoint itself stays a state-change PATCH.
        var html = $$"""
            <!doctype html>
            <html lang="es"><head><meta charset="utf-8"><title>Simular pago</title></head>
            <body style="font-family: sans-serif; max-width: 420px; margin: 60px auto; text-align:center;">
              <h2>Pasarela de pago (simulada)</h2>
              <p>Sesión: <code>{{sessionId}}</code></p>
              <p style="color:#666;font-size:0.9em;">TODO: reemplazar por un gateway real (Stripe u otro) cuando haya cuenta/credenciales.</p>
              <button onclick="settle('paid')" style="padding:10px 20px;background:#16a34a;color:#fff;border:none;border-radius:6px;cursor:pointer;">Pagar</button>
              <button onclick="settle('canceled')" style="padding:10px 20px;background:#dc2626;color:#fff;border:none;border-radius:6px;cursor:pointer;margin-left:8px">Cancelar</button>
              <script>
                async function settle(outcome) {
                  const res = await fetch('/api/v1/subscriptions/mock-checkout/{{sessionId}}', {
                    method: 'PATCH',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ outcome })
                  });
                  const data = await res.json();
                  if (data.redirectUrl) window.location.href = data.redirectUrl;
                }
              </script>
            </body></html>
            """;
        return Content(html, "text/html");
    }

    /// <summary>
    /// Settles a mock checkout session — the mock stand-in for a real gateway's webhook event.
    /// Outcome "paid" activates the subscription; "canceled" marks the payment failed.
    /// </summary>
    [HttpPatch("mock-checkout/{sessionId}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Settle a mock checkout session (paid or canceled)", OperationId = "SettleMockCheckout")]
    [SwaggerResponse(StatusCodes.Status200OK, "The session was settled; body has redirectUrl")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid outcome")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No subscription matches this session")]
    public async Task<IActionResult> SettleMockCheckout(
        string sessionId, [FromBody] UpdateMockCheckoutResource resource, CancellationToken cancellationToken)
    {
        switch (resource.Outcome.ToLowerInvariant())
        {
            case "paid":
                var activateResult = await commandService.Handle(
                    new ActivateSubscriptionCommand(sessionId, $"mock_sub_{Guid.NewGuid():N}", DateTimeOffset.UtcNow),
                    cancellationToken);
                return SubscriptionsActionResultAssembler.ToActionResultFromResult(
                    this, activateResult, errorLocalizer, problemDetailsFactory,
                    () => Ok(new { redirectUrl = paymentSettings.Value.SuccessUrl }));
            case "canceled":
                var failResult = await commandService.Handle(new MarkPaymentFailedCommand(sessionId), cancellationToken);
                return SubscriptionsActionResultAssembler.ToActionResultFromResult(
                    this, failResult, errorLocalizer, problemDetailsFactory,
                    () => Ok(new { redirectUrl = paymentSettings.Value.CancelUrl }));
            default:
                return problemDetailsFactory.CreateProblemDetails(
                    this, StatusCodes.Status400BadRequest, (Enum?)null,
                    $"'{resource.Outcome}' is not a valid outcome. Expected 'paid' or 'canceled'.");
        }
    }
}
