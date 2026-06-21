using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.CommandServices;

public interface ISensorReadingCommandService
{
    Task<Result<SensorReading>> Handle(RecordSensorReadingCommand command, CancellationToken cancellationToken);
}