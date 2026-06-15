namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record CreateProgressResource(
    int CompletedCheckpoints,
    int TotalCheckpoints,
    double Percentage);
    