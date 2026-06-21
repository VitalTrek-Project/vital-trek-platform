using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;

public interface ISensorReadingRepository : IBaseRepository<SensorReading>
{
    Task<IEnumerable<SensorReading>> FindByDeviceIdAsync(int deviceId, CancellationToken cancellationToken = default);
}