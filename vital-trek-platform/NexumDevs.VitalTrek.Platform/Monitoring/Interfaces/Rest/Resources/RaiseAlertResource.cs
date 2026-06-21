namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

public record RaiseAlertResource(
    int ExpeditionId,
    int TouristId,
    string Type,
    string Severity,
    string Message
);