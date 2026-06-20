namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;

public record RecordLocationCommand(
    int ExpeditionId,
    int TouristId,
    double Latitude,
    double Longitude,
    double AccuracyMeters
);