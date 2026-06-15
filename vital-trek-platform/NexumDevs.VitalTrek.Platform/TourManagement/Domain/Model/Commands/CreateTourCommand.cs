using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

/// <summary>
/// Command used to create a new tour.
/// </summary>
/// <param name="AgencyId">
/// The unique identifier of the agency that owns the tour.
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
/// <param name="Capacity">
/// The maximum number of tourists allowed to participate in the tour.
/// </param>
/// <param name="EstimatedDurationMinutes">
/// The estimated duration of the tour in minutes.
/// </param>
/// <param name="DistanceKm">
/// The total distance of the tour in kilometers.
/// </param>
public record CreateTourCommand(
    Guid AgencyId,
    string Title,
    string Description,
    EDifficultyLevel Difficulty,
    int Capacity,
    int EstimatedDurationMinutes,
    decimal DistanceKm);