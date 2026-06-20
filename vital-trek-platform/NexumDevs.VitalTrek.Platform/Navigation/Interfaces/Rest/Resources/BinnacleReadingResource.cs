namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record BinnacleReadingResource(
    int ExpeditionId,
    int TouristId,
    string Note,
    string MediaUrl,
    DateTime CreatedAt
);
