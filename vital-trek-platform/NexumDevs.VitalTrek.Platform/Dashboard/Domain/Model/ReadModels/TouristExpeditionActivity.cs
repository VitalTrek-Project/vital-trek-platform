namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record TouristExpeditionActivity(int ExpeditionId, string ExpeditionName, string Status, int GuideId, DateTimeOffset LastActivityAt);
