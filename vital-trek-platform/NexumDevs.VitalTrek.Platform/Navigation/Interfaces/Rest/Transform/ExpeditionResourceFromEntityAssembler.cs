using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class ExpeditionResourceFromEntityAssembler
{
    public static ExpeditionResource ToResourceFromEntity(Expedition entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Expedition entity cannot be null when converting to resource.");

        return new ExpeditionResource(
            entity.Id,
            entity.TourID.id,
            entity.GuideID.id,
            entity.ExpeditionName,
            entity.Status);
    }
}
