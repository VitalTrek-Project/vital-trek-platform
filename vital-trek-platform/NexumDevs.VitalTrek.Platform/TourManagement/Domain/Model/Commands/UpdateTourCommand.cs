using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

/// <summary>
/// Command used to update the basic information of an existing tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour to update.
/// </param>
/// <param name="Title">
/// The new title of the tour.
/// </param>
/// <param name="Description">
/// The new description of the tour.
/// </param>
public record UpdateTourCommand(
    Guid TourId,
    string Title,
    string Description);