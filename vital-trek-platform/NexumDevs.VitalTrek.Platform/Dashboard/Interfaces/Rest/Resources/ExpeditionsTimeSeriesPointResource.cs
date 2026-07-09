namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record ExpeditionsTimeSeriesPointResource(DateTimeOffset BucketStart, int Count);
