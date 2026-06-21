namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve all tourist assignments associated with a specific tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour whose assigned tourists are being requested.
/// </param>
public record GetAssignedTouristsByTourQuery(Guid TourId);