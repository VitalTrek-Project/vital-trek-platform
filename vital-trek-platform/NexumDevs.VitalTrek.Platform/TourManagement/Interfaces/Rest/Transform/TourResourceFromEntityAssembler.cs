using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Transform;

public static class TourResourceFromEntityAssembler
{
    public static TourResource ToResourceFromEntity(Tour entity)
    {
        return new TourResource(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Difficulty.ToString(),
            entity.Status.ToString(),
            entity.Capacity);
    }
}