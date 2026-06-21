using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

/// <summary>
/// Command used to create a duplicate of an existing tour.
/// </summary>
/// <param name="TourId">
/// The unique identifier of the tour to duplicate.
/// </param>
public record DuplicateTourCommand(Guid TourId);