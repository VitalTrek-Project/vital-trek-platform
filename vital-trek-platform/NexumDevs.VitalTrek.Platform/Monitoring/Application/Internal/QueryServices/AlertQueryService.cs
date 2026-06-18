using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository) : IAlertQueryService
{
    public async Task<IEnumerable<Alert>> Handle(GetActiveAlertsByExpeditionQuery query, CancellationToken cancellationToken)
    {
        return await alertRepository.FindActiveByExpeditionIdAsync(query.ExpeditionId, cancellationToken);
    }
}