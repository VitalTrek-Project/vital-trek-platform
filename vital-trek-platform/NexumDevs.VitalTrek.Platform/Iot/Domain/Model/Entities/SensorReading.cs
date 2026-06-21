using System;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;

public class SensorReading : AuditableModel
{
    //class SensorReading
    public SensorReading()
    {
    }

    public SensorReading(int deviceId, ESensorType type, double value, string? unit, DateTime recordedAt)
    {
        DeviceId = deviceId;
        Type = type;
        Value = value;
        Unit = unit;
        RecordedAt = recordedAt;
    }

    public int Id { get; set; }
    public int DeviceId { get; set; }
    public ESensorType Type { get; set; }
    public double Value { get; set; }
    public string? Unit { get; set; }
    public DateTime RecordedAt { get; set; }
}