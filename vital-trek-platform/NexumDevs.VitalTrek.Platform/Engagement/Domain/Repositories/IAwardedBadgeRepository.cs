using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IAwardedBadgeRepository : IBaseRepository<AwardedBadge>
{
    Task<IReadOnlyList<AwardedBadge>> FindByTouristAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid agencyId, Guid touristId, Guid badgeDefinitionId, CancellationToken cancellationToken);
}
