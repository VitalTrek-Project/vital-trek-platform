using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Application.QueryServices;

/**
 * <summary>
 *     The subscription query service
 * </summary>
 */
public interface ISubscriptionQueryService
{
    Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query, CancellationToken cancellationToken);
}
