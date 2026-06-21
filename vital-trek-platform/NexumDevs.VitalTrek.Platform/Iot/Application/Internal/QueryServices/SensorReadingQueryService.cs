using NexumDevs.VitalTrek.Platform.Iot.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.Internal.QueryServices;

public class SensorReadingQueryService(ISensorReadingRepository readingRepository) : ISensorReadingQueryService
{
    public async Task<IEnumerable<SensorReading>> Handle(GetAllSensorReadingsQuery query, CancellationToken cancellationToken)
    {
        return await readingRepository.ListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SensorReading>> Handle(GetSensorReadingsByDeviceQuery query, CancellationToken cancellationToken)
    {
        return await readingRepository.FindByDeviceIdAsync(query.DeviceId, cancellationToken);
    }
}