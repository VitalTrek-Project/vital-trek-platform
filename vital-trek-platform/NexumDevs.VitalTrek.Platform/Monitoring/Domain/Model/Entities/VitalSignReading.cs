using System;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;

public class VitalSignReading : AuditableModel
{
    public VitalSignReading()
    {
    }

    public VitalSignReading(int expeditionId, int touristId, int heartRate, double bloodOxygen, double bodyTemperature)
    {
        ExpeditionId = expeditionId;
        TouristId = touristId;
        HeartRate = heartRate;
        BloodOxygen = bloodOxygen;
        BodyTemperature = bodyTemperature;
        RecordedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int ExpeditionId { get; set; }
    public int TouristId { get; set; }
    public int HeartRate { get; set; }
    public double BloodOxygen { get; set; }
    public double BodyTemperature { get; set; }
    public DateTime RecordedAt { get; set; }

    // Evaluate if the reading is considered critical given thresholds
    public bool IsCritical(int minHr, int maxHr, double minSpO2, double maxTemp)
    {
        if (HeartRate < minHr || HeartRate > maxHr) return true;
        if (BloodOxygen < minSpO2) return true;
        if (BodyTemperature > maxTemp) return true;
        return false;
    }
}