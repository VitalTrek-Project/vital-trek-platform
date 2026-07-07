namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record ExpeditionActivitySummary(int ExpeditionId, string ExpeditionName, string Status, int GuideId, int TouristCount);
