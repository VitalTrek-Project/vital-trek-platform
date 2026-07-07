namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.Queries;

/// <summary>
/// Bucket is "week" or "month". Any other value falls back to "week" in the repository.
/// </summary>
public record GetExpeditionsTimeSeriesQuery(DateTimeOffset From, DateTimeOffset To, string Bucket);
