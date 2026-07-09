namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

/// <summary>
/// Body for PATCH /api/v1/alerts/{alertId} — a state-change request rather than a full
/// resource replacement. Status must be "ACKNOWLEDGED" or "DISMISSED"; UserId is required
/// when acknowledging (identifies who acknowledged the alert).
/// </summary>
public record UpdateAlertStatusResource(string Status, int? UserId);
