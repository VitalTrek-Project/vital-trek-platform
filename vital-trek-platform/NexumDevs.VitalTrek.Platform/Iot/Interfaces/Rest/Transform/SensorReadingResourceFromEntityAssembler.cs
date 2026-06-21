using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Transform;

public static class SensorReadingResourceFromEntityAssembler
{
    public static SensorReadingResource ToResourceFromEntity(SensorReading entity)
    {
        return new SensorReadingResource(
            entity.Id,
            entity.DeviceId,
            entity.Type.ToApiValue(),
            entity.Value,
            entity.Unit,
            entity.RecordedAt
        );
    }
}