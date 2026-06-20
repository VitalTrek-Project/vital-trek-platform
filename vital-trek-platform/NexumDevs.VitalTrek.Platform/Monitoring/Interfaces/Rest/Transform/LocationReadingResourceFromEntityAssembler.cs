using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;

public static class LocationReadingResourceFromEntityAssembler
{
    public static LocationReadingResource ToResourceFromEntity(LocationReading entity)
    {
        return new LocationReadingResource(
            entity.Id,
            entity.ExpeditionId,
            entity.TouristId,
            entity.Latitude,
            entity.Longitude,
            entity.AccuracyMeters,
            entity.RecordedAt
        );
    }
}