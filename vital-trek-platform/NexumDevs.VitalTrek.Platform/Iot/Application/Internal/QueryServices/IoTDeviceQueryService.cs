using NexumDevs.VitalTrek.Platform.Iot.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.Internal.QueryServices;

public class IoTDeviceQueryService(IIoTDeviceRepository deviceRepository) : IIoTDeviceQueryService
{
    public async Task<IEnumerable<IoTDevice>> Handle(GetAllDevicesQuery query, CancellationToken cancellationToken)
    {
        return await deviceRepository.ListAsync(cancellationToken);
    }

    public async Task<IoTDevice?> Handle(GetDeviceByIdQuery query, CancellationToken cancellationToken)
    {
        return await deviceRepository.FindByIdAsync(query.DeviceId, cancellationToken);
    }
}