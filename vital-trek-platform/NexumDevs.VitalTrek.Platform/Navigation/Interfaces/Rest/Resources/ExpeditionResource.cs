namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record ExpeditionResource(
    int Id,
    int TourID,
    int GuideID,
    string Status,
    string StartedAt,
    string FinishedAt);