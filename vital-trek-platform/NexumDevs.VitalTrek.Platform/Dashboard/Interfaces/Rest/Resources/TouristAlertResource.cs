namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record TouristAlertResource(
    int Id,
    string Type,
    string Severity,
    string Status,
    string Message,
    DateTimeOffset RaisedAt);
