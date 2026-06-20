
using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class IncidentRepository(AppDbContext context) : BaseRepository<Incident>(context), IIncidentRepository
{
    public new async Task<Incident?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Context.Set<Incident>()
            
            .FirstOrDefaultAsync(tutorial => tutorial.Id == id, cancellationToken);
    }
    
    public new async Task<IEnumerable<Incident>> ListAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<Incident>()
           
            .ToListAsync(cancellationToken);
    }

    public  Task<IEnumerable<Incident>> FindByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

