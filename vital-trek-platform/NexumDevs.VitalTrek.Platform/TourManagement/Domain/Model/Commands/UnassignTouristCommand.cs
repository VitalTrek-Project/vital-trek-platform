using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

public record UnassignTouristCommand(Guid TourId, Guid TouristId);

