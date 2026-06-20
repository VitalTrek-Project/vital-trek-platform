using System.Runtime.InteropServices.JavaScript;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

public record CreateWeatherCommand(
    int ExpeditionId,
    double TemperatureCelsius,
    string Condition,
    double Humidity,
    double WindSpeedKmh);
    