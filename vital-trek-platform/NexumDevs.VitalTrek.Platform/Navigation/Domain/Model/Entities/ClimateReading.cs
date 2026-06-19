namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

public class ClimateReading
{
    public ClimateReading () {}

    public ClimateReading (int expeditionId, double temperatureCelsius, string condition, double humidity, double windSpeedKnh)
    {
        ExpeditionId = expeditionId;
        TemperatureCelsius = temperatureCelsius;
        Condition = condition;
        Humidity = humidity;
        WindSpeedKnh = windSpeedKnh;
    }
    
    public int ExpeditionId { get; set; }
    public double TemperatureCelsius { get; set; }
    public string Condition { get; set; }
    public double Humidity { get; set; }
    public double WindSpeedKnh { get; set; }
}