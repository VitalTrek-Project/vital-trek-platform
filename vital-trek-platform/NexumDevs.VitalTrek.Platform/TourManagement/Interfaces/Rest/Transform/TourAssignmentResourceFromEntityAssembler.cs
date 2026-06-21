using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Transform;

public static class TourAssignmentResourceFromEntityAssembler
{
    public static TourAssignmentResource ToResourceFromEntity(TourAssignment entity)
    {
        return new TourAssignmentResource(
            entity.Id,
            entity.TourId,
            entity.TouristId,
            entity.Status.ToString(),
            entity.AssignedAt);
    }
}