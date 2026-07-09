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
 *     Outbound port to the payment gateway. Kept behind an interface so the Application
 *     layer stays free of any specific provider SDK (currently backed by a self-contained
 *     mock gateway for the demo — TODO: swap the Program.cs registration for a real provider
 *     like Stripe when credentials/time are available, without touching callers).
 * </summary>
 */
public interface IPaymentGatewayService
{
    Task<PaymentCheckoutSession> CreateCheckoutSessionAsync(
        Guid userId, SubscriptionPlan plan, string successUrl, string cancelUrl, CancellationToken cancellationToken);

    Task CancelSubscriptionAsync(string gatewaySubscriptionId, CancellationToken cancellationToken);
}
