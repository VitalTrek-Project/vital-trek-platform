using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

/// <summary>
/// Command used to assign a tourist to a tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour to which the tourist will be assigned.
/// </param>
/// <param name="TouristId">
/// The unique identifier of the tourist to assign.
/// </param>
public record AssignTouristCommand(
    Guid TourId,
    Guid TouristId);