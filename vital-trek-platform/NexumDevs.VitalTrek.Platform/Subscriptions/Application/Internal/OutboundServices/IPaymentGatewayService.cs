using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.OutboundServices;

/**
 * <summary>
 *     Result of starting a checkout session with the payment gateway.
 * </summary>
 */
public record PaymentCheckoutSession(string SessionId, string Url, string? CustomerId);

/**
 * <summary>
 *     A payment-gateway-agnostic view of a webhook event. Infrastructure translates the
 *     provider-specific payload (Stripe, etc.) into this shape so the Application layer
 *     never depends on a third-party SDK type.
 * </summary>
 */
public record PaymentWebhookEvent(string Type, string? CheckoutSessionId, string? StripeSubscriptionId, string? StripeCustomerId);

/**
 * <summary>
 *     Outbound port to the payment gateway (Stripe in production). Kept behind an
 *     interface so the Application layer stays free of the Stripe SDK.
 * </summary>
 */
public interface IPaymentGatewayService
{
    Task<PaymentCheckoutSession> CreateCheckoutSessionAsync(
        Guid userId, SubscriptionPlan plan, string successUrl, string cancelUrl, CancellationToken cancellationToken);

    PaymentWebhookEvent ParseWebhookEvent(string payload, string signatureHeader);

    Task CancelSubscriptionAsync(string stripeSubscriptionId, CancellationToken cancellationToken);
}
