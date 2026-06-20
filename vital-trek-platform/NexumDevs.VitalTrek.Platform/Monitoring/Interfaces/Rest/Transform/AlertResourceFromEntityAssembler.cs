using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert entity)
    {
        return new AlertResource(
            entity.Id,
            entity.TouristId,
            entity.Type.ToString(),
            entity.Severity.ToString(),
            entity.Status.ToString(),
            entity.Message,
            entity.CreatedAt ?? DateTimeOffset.UtcNow
        );
    }
}