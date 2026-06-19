namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record ExperienceResource(
    int Id,
    int ExpeditionID,
    int TouristID,
    string Note,
    string MediaUrl);
    