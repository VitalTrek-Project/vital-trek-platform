namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

public record CreateIncidentResource(
    int ExpeditionId, 
    int ReportedBy, 
    string Description,
    string Severity,
    string Status,
    string ReportedAt);