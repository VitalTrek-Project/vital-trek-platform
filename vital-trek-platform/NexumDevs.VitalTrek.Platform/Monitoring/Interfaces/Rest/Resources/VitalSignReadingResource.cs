namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

public record VitalSignReadingResource(
    int Id,
    int ExpeditionId,
    int TouristId,
    int HeartRate,
    double BloodOxygen,
    double BodyTemperature,
    DateTime RecordedAt
);