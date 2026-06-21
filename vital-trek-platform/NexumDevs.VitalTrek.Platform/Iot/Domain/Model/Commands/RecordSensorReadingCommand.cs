using System;

namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;

public record RecordSensorReadingCommand(
    int DeviceId,
    string Type,
    double Value,
    string? Unit,
    DateTime RecordedAt
);