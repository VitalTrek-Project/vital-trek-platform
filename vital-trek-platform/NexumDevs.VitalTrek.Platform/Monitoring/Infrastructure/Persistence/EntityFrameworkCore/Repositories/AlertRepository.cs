using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    public AlertRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Alert>> FindActiveByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken)
    {
        return await Context.Set<Alert>()
            .Where(a => a.ExpeditionId == expeditionId && a.Status == AlertStatus.ACTIVE)
            .ToListAsync(cancellationToken);
    }
}