namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record TouristExpeditionResource(int Id, string ExpeditionName, string Status, int GuideId, DateTimeOffset LastActivityAt);
