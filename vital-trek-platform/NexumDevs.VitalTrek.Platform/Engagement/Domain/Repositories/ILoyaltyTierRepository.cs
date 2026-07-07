using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface ILoyaltyTierRepository : IBaseRepository<LoyaltyTier>
{
    Task<IReadOnlyList<LoyaltyTier>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken);

    Task<LoyaltyTier?> FindByIdAndAgencyAsync(Guid tierId, Guid agencyId, CancellationToken cancellationToken);
}
