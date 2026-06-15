using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;

public record UpdateTourCommand(
    Guid TourId,
    string Title,
    string Description);