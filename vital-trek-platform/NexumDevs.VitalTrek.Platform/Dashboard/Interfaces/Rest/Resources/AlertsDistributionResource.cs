namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record AlertsDistributionResource(IDictionary<string, int> BySeverity, IDictionary<string, int> ByType);
