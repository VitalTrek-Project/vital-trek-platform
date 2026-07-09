using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GamificationProfileRepository(AppDbContext context)
    : BaseRepository<GamificationProfile>(context), IGamificationProfileRepository
{
    public async Task<GamificationProfile?> FindByTouristAndAgencyAsync(Guid touristId, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<GamificationProfile>()
            .FirstOrDefaultAsync(p => p.TouristId == touristId && p.AgencyId == agencyId, cancellationToken);
    }

    public async Task<IReadOnlyList<GamificationProfile>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<GamificationProfile>()
            .Where(p => p.AgencyId == agencyId)
            .ToListAsync(cancellationToken);
    }
}
