using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Infrastructure.Persistence.EFC.Repositories;

public class IoTDeviceRepository : BaseRepository<IoTDevice>, IIoTDeviceRepository
{
    public IoTDeviceRepository(AppDbContext context) : base(context)
    {
    }
}