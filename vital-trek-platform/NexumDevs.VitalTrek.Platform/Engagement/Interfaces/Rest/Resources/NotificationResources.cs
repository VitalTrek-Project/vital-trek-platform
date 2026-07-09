namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record NotificationResource(Guid Id, string Type, string Title, string Message, bool IsRead, DateTimeOffset CreatedAt);
