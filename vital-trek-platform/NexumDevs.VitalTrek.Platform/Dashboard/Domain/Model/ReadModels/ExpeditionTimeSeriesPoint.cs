namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record ExpeditionTimeSeriesPoint(DateTimeOffset BucketStart, int Count);
