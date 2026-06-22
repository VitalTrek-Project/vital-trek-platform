using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.CommandServices;
//iot
public interface IIoTDeviceCommandService
{
    Task<Result<IoTDevice>> Handle(RegisterDeviceCommand command, CancellationToken cancellationToken);
    Task<Result<IoTDevice>> Handle(RemoveDeviceCommand command, CancellationToken cancellationToken);
    Task<Result<IoTDevice>> Handle(DispatchDeviceCommandCommand command, CancellationToken cancellationToken);
}