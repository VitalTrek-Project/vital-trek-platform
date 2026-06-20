using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Weather
{
    public Weather()
    {
        TemperatureCelsius = 0.0;
        Condition = null!;
        Humidity = 0.0;
        WindSpeedKmh = 0.0;
    }
    
    public Weather(CreateWeatherCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        TemperatureCelsius = command.TemperatureCelsius;
        Condition = command.Condition;
        Humidity = command.Humidity;
        WindSpeedKmh = command.WindSpeedKmh;
    }
    
    public int Id { get; }
    
    public double TemperatureCelsius { get; private set; }
    public string Condition { get; private set; }
    public double Humidity { get; private set; }
    public double WindSpeedKmh { get; private set; }
}
