using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class LocationReadingRepository : BaseRepository<LocationReading>, ILocationReadingRepository
{
    public LocationReadingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LocationReading>> FindByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken)
    {
        return await Context.Set<LocationReading>()
            .Where(l => l.ExpeditionId == expeditionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<LocationReading?> FindLatestByTouristAsync(int touristId, int expeditionId, CancellationToken cancellationToken)
    {
        return await Context.Set<LocationReading>()
            .Where(l => l.TouristId == touristId && l.ExpeditionId == expeditionId)
            .OrderByDescending(l => l.RecordedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}