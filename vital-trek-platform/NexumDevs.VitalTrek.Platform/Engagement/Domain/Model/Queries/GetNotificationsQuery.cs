namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

public record GetNotificationsQuery(Guid TouristId, bool? UnreadOnly);
