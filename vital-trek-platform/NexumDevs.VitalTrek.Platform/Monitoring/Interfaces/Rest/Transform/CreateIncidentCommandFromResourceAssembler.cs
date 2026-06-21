using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;

public class CreateIncidentCommandFromResourceAssembler
{
    public static CreateIncidentCommand ToCommandFromResource(CreateIncidentResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateIncidentResource cannot be null when converting to command.");
        return new CreateIncidentCommand(resource.ExpeditionId,
            resource.ReportedBy, resource.Description, resource.Severity, resource.Status, resource.ReportedAt);
    }
}