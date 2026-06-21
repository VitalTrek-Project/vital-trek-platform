namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;

public static class IoTEnumExtensions
{
    // --- Enum → API string (used in assemblers) ---

    public static string ToApiValue(this EDeviceType type) => type switch
    {
        EDeviceType.Wearable => "wearable",
        EDeviceType.GpsTracker => "gps_tracker",
        EDeviceType.Environmental => "environmental",
        _ => type.ToString().ToLowerInvariant()
    };

    public static string ToApiValue(this EDeviceStatus status) => status switch
    {
        EDeviceStatus.Online => "online",
        EDeviceStatus.Offline => "offline",
        _ => status.ToString().ToLowerInvariant()
    };

    public static string ToApiValue(this ESensorType sensorType) => sensorType switch
    {
        ESensorType.HeartRate => "heart_rate",
        ESensorType.BloodOxygen => "blood_oxygen",
        ESensorType.BodyTemperature => "body_temperature",
        ESensorType.Temperature => "temperature",
        ESensorType.Steps => "steps",
        ESensorType.Location => "location",
        ESensorType.Altitude => "altitude",
        _ => sensorType.ToString().ToLowerInvariant()
    };

    // --- API string → Enum (used in command services, returns null on unknown value) ---

    public static EDeviceType? ParseDeviceType(string? value) => value switch
    {
        "wearable" => EDeviceType.Wearable,
        "gps_tracker" => EDeviceType.GpsTracker,
        "environmental" => EDeviceType.Environmental,
        _ => null
    };

    public static EDeviceStatus? ParseDeviceStatus(string? value) => value switch
    {
        "online" => EDeviceStatus.Online,
        "offline" => EDeviceStatus.Offline,
        _ => null
    };

    public static ESensorType? ParseSensorType(string? value) => value switch
    {
        "heart_rate" => ESensorType.HeartRate,
        "blood_oxygen" => ESensorType.BloodOxygen,
        "body_temperature" => ESensorType.BodyTemperature,
        "temperature" => ESensorType.Temperature,
        "steps" => ESensorType.Steps,
        "location" => ESensorType.Location,
        "altitude" => ESensorType.Altitude,
        _ => null
    };
}