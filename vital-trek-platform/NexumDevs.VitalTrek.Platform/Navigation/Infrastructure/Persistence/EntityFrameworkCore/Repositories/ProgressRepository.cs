using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProgressRepository(AppDbContext context) : BaseRepository<Progress>(context), IProgressRepository
{
    public async Task<IEnumerable<Progress>> FindExperienceByExpeditionIdAsync(int expeditionId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<Progress>()
            .Include(progress => progress.Expedition)
            .Where(progress => progress.ExpeditionId == expeditionId)
            .ToListAsync(cancellationToken);
    }
}
