namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record WeatherResource(
    int Id,
    double TemperatureCelsius,
    string Condition,
    double Humidity,
    double WindSpeedKmh);
    