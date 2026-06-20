using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(GetActiveAlertsByExpeditionQuery query, CancellationToken cancellationToken);
}