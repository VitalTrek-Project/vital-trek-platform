namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents a tour assignment resource returned by the API.
/// </summary>
/// <param name="Id">
/// The unique identifier of the assignment.
/// </param>
/// <param name="TourId">
/// The unique identifier of the associated tour.
/// </param>
/// <param name="TouristId">
/// The unique identifier of the assigned tourist.
/// </param>
/// <param name="Status">
/// The current status of the assignment.
/// </param>
/// <param name="AssignedAt">
/// The date and time when the tourist was assigned to the tour.
/// </param>
public record TourAssignmentResource(
    Guid Id,
    Guid TourId,
    Guid TouristId,
    string Status,
    DateTimeOffset AssignedAt);