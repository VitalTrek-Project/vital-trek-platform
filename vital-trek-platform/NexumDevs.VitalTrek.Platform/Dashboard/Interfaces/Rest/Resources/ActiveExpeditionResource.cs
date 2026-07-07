namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record ActiveExpeditionResource(int Id, string ExpeditionName, string Status, int GuideId, int TouristCount);
