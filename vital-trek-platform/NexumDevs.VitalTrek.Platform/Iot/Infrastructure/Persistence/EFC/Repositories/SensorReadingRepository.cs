using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Infrastructure.Persistence.EFC.Repositories;

public class SensorReadingRepository : BaseRepository<SensorReading>, ISensorReadingRepository
{
    public SensorReadingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SensorReading>> FindByDeviceIdAsync(int deviceId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<SensorReading>()
            .Where(r => r.DeviceId == deviceId)
            .ToListAsync(cancellationToken);
    }
}