using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class NotificationResourceAssembler
{
    public static NotificationResource ToResourceFromEntity(InAppNotification entity) =>
        new(entity.Id, entity.Type.ToString(), entity.Title, entity.Message, entity.IsRead, entity.CreatedAt);
}
