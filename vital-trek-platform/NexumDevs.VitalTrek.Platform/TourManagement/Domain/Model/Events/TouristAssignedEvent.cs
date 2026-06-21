namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;

/// <summary>
/// Domain event raised when a tourist is assigned to a tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour to which the tourist was assigned.
/// </param>
/// <param name="TouristId">
/// The unique identifier of the assigned tourist.
/// </param>
/// <param name="AssignedAt">
/// The date and time when the assignment occurred.
/// </param>
public record TouristAssignedEvent(
    Guid TourId,
    Guid TouristId,
    DateTimeOffset AssignedAt);