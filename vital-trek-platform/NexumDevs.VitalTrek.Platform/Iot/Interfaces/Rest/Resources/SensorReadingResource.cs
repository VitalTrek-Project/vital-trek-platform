namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

public record SensorReadingResource(
    int Id,
    int DeviceId,
    string Type,
    double Value,
    string? Unit,
    DateTime RecordedAt
);