using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IReferralRepository : IBaseRepository<Referral>
{
    Task<Referral?> FindPendingByReferredTouristAsync(Guid agencyId, Guid referredTouristId, CancellationToken cancellationToken);

    Task<int> CountCompletedByReferrerAsync(Guid agencyId, Guid referrerTouristId, CancellationToken cancellationToken);
}
