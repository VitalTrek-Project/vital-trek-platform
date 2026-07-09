namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record AttentionAlertResource(
    int Id,
    int TouristId,
    int ExpeditionId,
    string Type,
    string Severity,
    string Message,
    DateTimeOffset RaisedAt);
