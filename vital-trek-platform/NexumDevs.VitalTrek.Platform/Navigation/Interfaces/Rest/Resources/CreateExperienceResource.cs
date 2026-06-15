namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record CreateExperienceResource(
    int ExpeditionID,
    int TouristID,
    string Note,
    string MediaUrl,
    string CreatedAt);
    