using Microsoft.Extensions.Options;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.OutboundServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Stripe.Configuration;
using Stripe;
using Stripe.Checkout;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Stripe.Services;

/**
 * <summary>
 *     Stripe.net-backed implementation of <see cref="IPaymentGatewayService" />. Uses Stripe
 *     Checkout (hosted page) in subscription mode so the platform never touches raw card data.
 * </summary>
 */
public class StripePaymentGatewayService(IOptions<StripeSettings> options) : IPaymentGatewayService
{
    private readonly StripeSettings _settings = options.Value;

    public async Task<PaymentCheckoutSession> CreateCheckoutSessionAsync(
        Guid userId, SubscriptionPlan plan, string successUrl, string cancelUrl, CancellationToken cancellationToken)
    {
        var client = EnsureConfigured();
        var (amountCents, currency, displayName) = PlanCatalog.Get(plan);

        var createOptions = new SessionCreateOptions
        {
            Mode = "subscription",
            PaymentMethodTypes = ["card"],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = amountCents,
                        ProductData = new SessionLineItemPriceDataProductDataOptions { Name = displayName },
                        Recurring = new SessionLineItemPriceDataRecurringOptions
                        {
                            Interval = plan == SubscriptionPlan.Monthly ? "month" : "year"
                        }
                    }
                }
            ],
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            ClientReferenceId = userId.ToString(),
            Metadata = new Dictionary<string, string> { ["userId"] = userId.ToString() }
        };

        var service = new SessionService(client);
        var session = await service.CreateAsync(createOptions, cancellationToken: cancellationToken);
        return new PaymentCheckoutSession(session.Id, session.Url, session.CustomerId);
    }

    public PaymentWebhookEvent ParseWebhookEvent(string payload, string signatureHeader)
    {
        EnsureConfigured();
        if (string.IsNullOrWhiteSpace(_settings.WebhookSecret))
            throw new PaymentGatewayNotConfiguredException(
                "Stripe webhook secret is not configured (Stripe:WebhookSecret / env var Stripe__WebhookSecret).");

        var stripeEvent = EventUtility.ConstructEvent(payload, signatureHeader, _settings.WebhookSecret);

        string? checkoutSessionId = null;
        string? subscriptionId = null;
        string? customerId = null;

        switch (stripeEvent.Data.Object)
        {
            case Session session:
                checkoutSessionId = session.Id;
                subscriptionId = session.SubscriptionId;
                customerId = session.CustomerId;
                break;
            case Invoice invoice:
                subscriptionId = invoice.Parent?.SubscriptionDetails?.SubscriptionId;
                customerId = invoice.CustomerId;
                break;
            case global::Stripe.Subscription subscription:
                subscriptionId = subscription.Id;
                customerId = subscription.CustomerId;
                break;
        }

        return new PaymentWebhookEvent(stripeEvent.Type, checkoutSessionId, subscriptionId, customerId);
    }

    public async Task CancelSubscriptionAsync(string stripeSubscriptionId, CancellationToken cancellationToken)
    {
        var client = EnsureConfigured();
        var service = new SubscriptionService(client);
        await service.CancelAsync(stripeSubscriptionId, cancellationToken: cancellationToken);
    }

    private StripeClient EnsureConfigured()
    {
        if (string.IsNullOrWhiteSpace(_settings.SecretKey))
            throw new PaymentGatewayNotConfiguredException(
                "Stripe is not configured yet. Set Stripe:SecretKey (env var Stripe__SecretKey) to enable checkout.");

        return new StripeClient(_settings.SecretKey);
    }
}
