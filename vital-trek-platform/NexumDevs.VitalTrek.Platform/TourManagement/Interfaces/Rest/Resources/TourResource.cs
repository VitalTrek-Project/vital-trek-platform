namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents a tour resource returned by the API.
/// </summary>
/// <param name="Id">
/// The unique identifier of the tour.
/// </param>
/// <param name="Title">
/// The title of the tour.
/// </param>
/// <param name="Description">
/// A detailed description of the tour.
/// </param>
/// <param name="Difficulty">
/// The difficulty level of the tour.
/// </param>
/// <param name="Status">
/// The current status of the tour.
/// </param>
/// <param name="Capacity">
/// The maximum number of tourists allowed to participate in the tour.
/// </param>
public record TourResource(
    Guid Id,
    string Title,
    string Description,
    string Difficulty,
    string Status,
    int Capacity);