using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Transform;

public static class TourCommandFromResourceAssembler
{
    public static CreateTourCommand ToCreateCommandFromResource(CreateTourResource resource)
    {
        if (!Enum.TryParse<EDifficultyLevel>(resource.Difficulty, ignoreCase: true, out var difficulty))
            throw new TourManagementError(
                TourManagementErrors.InvalidDifficulty,
                $"'{resource.Difficulty}' is not a valid difficulty. Expected one of: Easy, Moderate, Hard, Expert.");

        return new CreateTourCommand(
            resource.AgencyId,
            resource.Title,
            resource.Description,
            difficulty,
            resource.Capacity,
            resource.EstimatedDurationMinutes,
            resource.DistanceKm);
    }

    public static UpdateTourCommand ToUpdateCommandFromResource(UpdateTourResource resource, Guid tourId)
    {
        return new UpdateTourCommand(tourId, resource.Title, resource.Description);
    }
}