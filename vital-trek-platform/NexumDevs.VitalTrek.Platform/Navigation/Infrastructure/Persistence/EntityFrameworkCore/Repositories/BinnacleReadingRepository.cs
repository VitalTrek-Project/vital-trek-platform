using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BinnacleReadingRepository(AppDbContext context)
    : BaseRepository<BinnacleReading>(context), IBinnacleReadingRepository
{
    public async Task<IEnumerable<BinnacleReading>> FindByExpeditionIdAsync(
        int expeditionId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<BinnacleReading>()
            .Where(b => b.ExpeditionId == expeditionId)
            .ToListAsync(cancellationToken);
    }
}
