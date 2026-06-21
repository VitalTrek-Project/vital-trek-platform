using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;

public interface ILocationReadingQueryService
{
    Task<IEnumerable<LocationReading>> Handle(GetLocationReadingsByExpeditionQuery query, CancellationToken cancellationToken);
}