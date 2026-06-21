namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents the request resource used to update an existing tour.
/// </summary>
/// <param name="Title">
/// The updated title of the tour.
/// </param>
/// <param name="Description">
/// The updated description of the tour.
/// </param>
public record UpdateTourResource(
    string Title,
    string Description);