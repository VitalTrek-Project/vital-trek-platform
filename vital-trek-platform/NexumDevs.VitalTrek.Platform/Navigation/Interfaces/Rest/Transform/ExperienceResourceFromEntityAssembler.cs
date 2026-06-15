using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class ExperienceResourceFromEntityAssembler
{
    public static ExperienceResource ToResourceFromEntity(Experience entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Experience cannot be null when converting to resource.");
        
        return new ExperienceResource(
            entity.Id,
            entity.ExpeditionID.id,
            entity.TouristID.id,
            entity.Note.Content,
            entity.MediaUrl, 
            entity.CreatedAt);
    }
}
