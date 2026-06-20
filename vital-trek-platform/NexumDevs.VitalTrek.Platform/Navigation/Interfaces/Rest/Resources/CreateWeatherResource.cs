namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record CreateWeatherResource(
    int ExpeditionId,
    double TemperatureCelsius,
    string Condition,
    double Humidity,
    double WindSpeedKmh);
    