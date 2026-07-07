namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record AlertsDistribution(IDictionary<string, int> BySeverity, IDictionary<string, int> ByType);
