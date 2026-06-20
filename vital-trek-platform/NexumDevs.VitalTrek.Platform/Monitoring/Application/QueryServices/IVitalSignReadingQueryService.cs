using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;

public interface IVitalSignReadingQueryService
{
    Task<IEnumerable<VitalSignReading>> Handle(GetVitalSignReadingsByExpeditionQuery query, CancellationToken cancellationToken);
}