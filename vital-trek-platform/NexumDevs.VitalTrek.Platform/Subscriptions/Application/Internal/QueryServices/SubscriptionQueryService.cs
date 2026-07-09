using NexumDevs.VitalTrek.Platform.Subscriptions.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.QueryServices;

/**
 * <summary>
 *     The subscription query service implementation
 * </summary>
 */
public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await subscriptionRepository.FindByUserIdAsync(query.UserId, cancellationToken);
    }
}
