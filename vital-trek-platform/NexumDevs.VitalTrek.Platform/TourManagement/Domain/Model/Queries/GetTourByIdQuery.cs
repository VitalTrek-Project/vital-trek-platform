namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve a tour by its unique identifier.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour to retrieve.
/// </param>
public record GetTourByIdQuery(Guid TourId);