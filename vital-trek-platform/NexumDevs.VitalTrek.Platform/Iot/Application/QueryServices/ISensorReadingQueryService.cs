using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.QueryServices;

public interface ISensorReadingQueryService
{
    Task<IEnumerable<SensorReading>> Handle(GetAllSensorReadingsQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<SensorReading>> Handle(GetSensorReadingsByDeviceQuery query, CancellationToken cancellationToken);
}