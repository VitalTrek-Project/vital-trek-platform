using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IRewardRepository : IBaseRepository<Reward>
{
    Task<IReadOnlyList<Reward>> FindByAgencyIdAsync(Guid agencyId, bool? activeOnly, CancellationToken cancellationToken);

    Task<Reward?> FindByIdAndAgencyAsync(Guid rewardId, Guid agencyId, CancellationToken cancellationToken);
}
