namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record CreateExpeditionResource(
    int TourID,
    int GuideID,
    string Status,
    string StartedAt,
    string FinishedAt);
    