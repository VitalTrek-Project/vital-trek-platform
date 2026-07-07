using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IGamificationProfileRepository : IBaseRepository<GamificationProfile>
{
    Task<GamificationProfile?> FindByTouristAndAgencyAsync(Guid touristId, Guid agencyId, CancellationToken cancellationToken);

    Task<IReadOnlyList<GamificationProfile>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken);
}
