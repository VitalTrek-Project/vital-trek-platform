using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public static class BinnacleReadingResourceFromEntityAssembler
{
    public static BinnacleReadingResource ToResourceFromEntity(
        BinnacleReading entity)
    {
        return new BinnacleReadingResource(
            entity.ExpeditionId,
            entity.TouristId,
            entity.Note,
            entity.MediaUrl,
            entity.CreatedAt
        );
    }
}
