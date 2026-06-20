namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

public record AlertResource(
    int Id,
    int TouristId,
    string Type,
    string Severity,
    string Status,
    string Message,
    DateTimeOffset RaisedAt
);