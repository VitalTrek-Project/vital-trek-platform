using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ExpeditionRepository(AppDbContext context) : BaseRepository<Expedition>(context), IExpeditionRepository
{
    public async Task<bool> ExistsByExpeditionNameAsync(string expeditionName, CancellationToken cancellationToken)
    {
        return await Context.Set<Expedition>().AnyAsync(expedition => expedition.ExpeditionName == expeditionName, cancellationToken);
    }
}