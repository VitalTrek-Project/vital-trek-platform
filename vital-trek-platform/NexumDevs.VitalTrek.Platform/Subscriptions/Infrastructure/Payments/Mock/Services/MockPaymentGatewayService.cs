using Microsoft.AspNetCore.Http;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.OutboundServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Mock.Services;

/**
 * <summary>
 *     TODO: academic-demo stand-in for a real payment gateway (e.g. Stripe). Instead of calling
 *     out to a third party, it points the caller back at our own "mock-checkout" page
 *     (<see cref="SubscriptionsController" />), so the whole subscription flow works end-to-end
 *     with zero external dependencies or credentials. Swap the Program.cs registration for a
 *     real <see cref="IPaymentGatewayService" /> implementation later — callers never change.
 * </summary>
 */
public class MockPaymentGatewayService(IHttpContextAccessor httpContextAccessor) : IPaymentGatewayService
{
    public Task<PaymentCheckoutSession> CreateCheckoutSessionAsync(
        Guid userId, SubscriptionPlan plan, string successUrl, string cancelUrl, CancellationToken cancellationToken)
    {
        var sessionId = $"mock_cs_{Guid.NewGuid():N}";
        var request = httpContextAccessor.HttpContext!.Request;
        var checkoutUrl = $"{request.Scheme}://{request.Host}/api/v1/subscriptions/mock-checkout/{sessionId}";

        return Task.FromResult(new PaymentCheckoutSession(sessionId, checkoutUrl, CustomerId: null));
    }

    public Task CancelSubscriptionAsync(string gatewaySubscriptionId, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
