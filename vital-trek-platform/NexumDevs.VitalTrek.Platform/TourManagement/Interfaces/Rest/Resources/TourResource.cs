namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

public record TourResource(
    Guid Id,
    string Title,
    string Description,
    string Difficulty,
    string Status,
    int Capacity);