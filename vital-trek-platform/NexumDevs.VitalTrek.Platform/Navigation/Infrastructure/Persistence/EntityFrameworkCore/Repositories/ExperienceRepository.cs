using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ExperienceRepository(AppDbContext context) : BaseRepository<Experience>(context), IExperienceRepository
{
    public async Task<IEnumerable<Experience>> FindExperienceByExpeditionIdAsync(int expeditionId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<Experience>()
            .Include(experience => experience.Expedition)
            .Where(experience => experience.ExpeditionID == expeditionId)
            .ToListAsync<Experience>(cancellationToken);
    }
}
