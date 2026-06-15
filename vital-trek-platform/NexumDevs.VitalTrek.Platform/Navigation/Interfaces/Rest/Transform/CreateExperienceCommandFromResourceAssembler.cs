using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class CreateExperienceCommandFromResourceAssembler
{
    public static CreateExperienceCommand ToCommandFromResource(CreateExperienceResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateExperienceResource cannot be null when converting to command.");
        return new CreateExperienceCommand(new ExpeditionId(resource.ExpeditionID), 
            new TouristId(resource.TouristID), new NoteItem(resource.Note), resource.MediaUrl, resource.CreatedAt);
    }
}