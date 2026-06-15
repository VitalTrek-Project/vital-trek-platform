using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public class WeatherResourceFromEntityAssembler
{
    public static WeatherResource ToResourceFromEntity(Weather entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Weather entity cannot be null when converting to resource.");
        
        return new WeatherResource(
            entity.Id,
            entity.TemperatureCelsius,
            entity.Condition,
            entity.Humidity,
            entity.WindSpeedKmh
        );
    }
}