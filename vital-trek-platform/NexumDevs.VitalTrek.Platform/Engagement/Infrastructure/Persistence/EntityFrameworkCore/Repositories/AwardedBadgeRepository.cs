using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AwardedBadgeRepository(AppDbContext context)
    : BaseRepository<AwardedBadge>(context), IAwardedBadgeRepository
{
    public async Task<IReadOnlyList<AwardedBadge>> FindByTouristAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken)
    {
        return await Context.Set<AwardedBadge>()
            .Where(b => b.AgencyId == agencyId && b.TouristId == touristId)
            .OrderByDescending(b => b.AwardedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid agencyId, Guid touristId, Guid badgeDefinitionId, CancellationToken cancellationToken)
    {
        return await Context.Set<AwardedBadge>().AnyAsync(
            b => b.AgencyId == agencyId && b.TouristId == touristId && b.BadgeDefinitionId == badgeDefinitionId,
            cancellationToken);
    }
}
