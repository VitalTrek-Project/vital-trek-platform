using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Errors;

public static class MonitoringErrors
{
    public static readonly Error IncidentCreationFailed =
        new("Monitoring.IncidentCreationFailed", "An error occurred while creating the incident.");
}