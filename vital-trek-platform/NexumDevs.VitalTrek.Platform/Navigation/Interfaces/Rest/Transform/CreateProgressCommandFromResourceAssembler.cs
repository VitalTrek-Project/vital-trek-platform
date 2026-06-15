using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class CreateProgressCommandFromResourceAssembler
{
    public static CreateProgressCommand ToCommandFromResource(CreateProgressResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateProgressResource cannot be null when converting to command.");
        return new CreateProgressCommand(resource.CompletedCheckpoints, 
            resource.TotalCheckpoints, resource.Percentage);
    }
}
