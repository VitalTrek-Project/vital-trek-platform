using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class CreateWeatherCommandFromResourceAssembler
{
    public static CreateWeatherCommand ToCommandFromResource(CreateWeatherResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateWeatherResource cannot be null when converting to command.");
        
        return new CreateWeatherCommand(resource.TemperatureCelsius, resource.Condition,
            resource.Humidity, resource.WindSpeedKmh);
    }
}
