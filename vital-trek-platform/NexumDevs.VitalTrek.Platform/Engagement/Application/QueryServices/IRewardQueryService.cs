using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface IRewardQueryService
{
    Task<IReadOnlyList<Reward>> Handle(GetRewardsQuery query, CancellationToken cancellationToken);
}
