namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;

public record LocationReadingResource(
    int Id,
    int ExpeditionId,
    int TouristId,
    double Latitude,
    double Longitude,
    double AccuracyMeters,
    DateTime RecordedAt
);