using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class ProgressResourceFromEntityAssembler
{
    public static ProgressResource ToResource(Progress entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Progress entity cannot be null when converting to resource.");
        
        return new ProgressResource
        (
            entity.Id,
            entity.CompletedCheckpoints,
            entity.TotalCheckpoints,
            entity.Percentage
        );
    }
}
