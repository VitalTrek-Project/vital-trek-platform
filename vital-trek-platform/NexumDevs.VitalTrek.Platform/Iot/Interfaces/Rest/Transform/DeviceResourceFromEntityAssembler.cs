using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Transform;

public static class DeviceResourceFromEntityAssembler
{
    public static DeviceResource ToResourceFromEntity(IoTDevice entity)
    {
        return new DeviceResource(
            entity.Id,
            entity.Name,
            entity.Type.ToApiValue(),
            entity.Status.ToApiValue(),
            entity.LastSeen,
            entity.LastCommand,
            entity.ExpeditionId,
            entity.TouristId
        );
    }
}