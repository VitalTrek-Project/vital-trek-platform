using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.QueryServices;

public interface IIoTDeviceQueryService
{
    Task<IEnumerable<IoTDevice>> Handle(GetAllDevicesQuery query, CancellationToken cancellationToken);
    Task<IoTDevice?> Handle(GetDeviceByIdQuery query, CancellationToken cancellationToken);
}