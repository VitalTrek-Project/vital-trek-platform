namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record CreateExpeditionResource(
    int TourID,
    int GuideID,
    string ExpeditionName,
    string Status);
    