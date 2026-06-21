namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;

public record RecordVitalSignsCommand(
    int ExpeditionId,
    int TouristId,
    int HeartRate,
    double BloodOxygen,
    double BodyTemperature
);