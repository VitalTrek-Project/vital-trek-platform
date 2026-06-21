namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;

public record ProgressResource(
    int Id,
    int ExpeditionId,
    int CompletedCheckpoints,
    int TotalCheckpoints,
    double Percentage);
    