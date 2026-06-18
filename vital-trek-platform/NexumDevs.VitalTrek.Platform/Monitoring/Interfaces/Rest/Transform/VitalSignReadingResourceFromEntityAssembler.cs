using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;

public static class VitalSignReadingResourceFromEntityAssembler
{
    public static VitalSignReadingResource ToResourceFromEntity(VitalSignReading entity)
    {
        return new VitalSignReadingResource(
            entity.Id,
            entity.ExpeditionId,
            entity.TouristId,
            entity.HeartRate,
            entity.BloodOxygen,
            entity.BodyTemperature,
            entity.RecordedAt
        );
    }
}