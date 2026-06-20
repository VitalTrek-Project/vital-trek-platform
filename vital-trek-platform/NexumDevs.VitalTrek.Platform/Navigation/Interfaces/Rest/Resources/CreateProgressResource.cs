namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record CreateProgressResource(
    int ExpeditionId,
    int CompletedCheckpoints,
    int TotalCheckpoints,
    double Percentage);
    