namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents the request resource used to assign a tourist to a tour.
/// </summary>
/// <param name="TouristId">
/// The unique identifier of the tourist to be assigned.
/// </param>
public record AssignTouristResource(Guid TouristId);