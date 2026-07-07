using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class RewardQueryService(IRewardRepository rewardRepository) : IRewardQueryService
{
    public async Task<IReadOnlyList<Reward>> Handle(GetRewardsQuery query, CancellationToken cancellationToken)
    {
        return await rewardRepository.FindByAgencyIdAsync(query.AgencyId, query.ActiveOnly, cancellationToken);
    }
}
