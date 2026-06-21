namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

public record RecordSensorReadingResource(
    int DeviceId,
    string Type,
    double Value,
    string? Unit,
    DateTime RecordedAt
);