using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;

public class IncidentResourceFromEntityAssembler
{
    public static IncidentResource ToResourceFromEntity(Incident entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Incident entity cannot be null when converting to resource.");

     

        return new IncidentResource(
            entity.Id,
            entity.ExpeditionId,
            entity.ReportedBy,
            entity.Description,
            entity.Severity,
            entity.Status,
            entity.ReportedAt
            
            );
    }
}