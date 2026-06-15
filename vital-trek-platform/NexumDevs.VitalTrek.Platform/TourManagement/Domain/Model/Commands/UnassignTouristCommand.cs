using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

/// <summary>
/// Command used to remove a tourist assignment from a tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour from which the tourist will be unassigned.
/// </param>
/// <param name="TouristId">
/// The unique identifier of the tourist to unassign.
/// </param>
public record UnassignTouristCommand(
    Guid TourId,
    Guid TouristId);