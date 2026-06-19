using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class CreateExpeditionCommandFromResourceAssembler
{
    public static CreateExpeditionCommand ToCommandFromResource(CreateExpeditionResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateExpeditionResource cannot be null when converting to command.");
        return new CreateExpeditionCommand(new TourId(resource.TourID), new GuideId(resource.GuideID), resource.ExpeditionName,
            resource.Status);
    }
}
