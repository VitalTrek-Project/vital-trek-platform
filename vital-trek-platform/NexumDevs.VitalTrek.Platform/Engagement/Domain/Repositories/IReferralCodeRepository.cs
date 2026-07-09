using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IReferralCodeRepository : IBaseRepository<ReferralCode>
{
    Task<ReferralCode?> FindByTouristAndAgencyAsync(Guid touristId, Guid agencyId, CancellationToken cancellationToken);

    Task<ReferralCode?> FindByCodeAsync(Guid agencyId, string code, CancellationToken cancellationToken);
}
