using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

/// <summary>
/// Command used to delete an existing tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour to be deleted.
/// </param>
public record DeleteTourCommand(Guid TourId);