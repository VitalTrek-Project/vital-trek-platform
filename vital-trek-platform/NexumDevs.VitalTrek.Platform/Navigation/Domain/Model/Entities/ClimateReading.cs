namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

public class ClimateReading
{
    public ClimateReading () {}

    public ClimateReading (int expeditionId, double temperatureCelsius, string condition, double humidity, double windSpeedKmh)
    {
        ExpeditionId = expeditionId;
        TemperatureCelsius = temperatureCelsius;
        Condition = condition;
        Humidity = humidity;
        WindSpeedKmh = windSpeedKmh;
    }
    
    public int ExpeditionId { get; set; }
    public double TemperatureCelsius { get; set; }
    public string Condition { get; set; }
    public double Humidity { get; set; }
    public double WindSpeedKmh { get; set; }
}
