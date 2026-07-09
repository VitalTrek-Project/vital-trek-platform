namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record KpiResource(int Value, int PreviousValue, double DeltaPercentage);
