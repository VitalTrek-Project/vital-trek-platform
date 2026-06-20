using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VitalSignReadingRepository : BaseRepository<VitalSignReading>, IVitalSignReadingRepository
{
    public VitalSignReadingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<VitalSignReading>> FindByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken)
    {
        return await Context.Set<VitalSignReading>()
            .Where(v => v.ExpeditionId == expeditionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<VitalSignReading?> FindLatestByTouristAsync(int touristId, int expeditionId, CancellationToken cancellationToken)
    {
        return await Context.Set<VitalSignReading>()
            .Where(v => v.TouristId == touristId && v.ExpeditionId == expeditionId)
            .OrderByDescending(v => v.RecordedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}