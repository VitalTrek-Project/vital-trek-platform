using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Repositories;

/**
 * <summary>
 *     The subscription repository
 * </summary>
 */
public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    /**
     * <summary>
     *     Find the most recent subscription for a user
     * </summary>
     */
    Task<Subscription?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /**
     * <summary>
     *     Find the subscription tied to a Stripe Checkout session
     * </summary>
     */
    Task<Subscription?> FindByStripeSessionIdAsync(string stripeCheckoutSessionId, CancellationToken cancellationToken);
}
