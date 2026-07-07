using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class NotificationQueryService(IInAppNotificationRepository notificationRepository) : INotificationQueryService
{
    public async Task<IReadOnlyList<InAppNotification>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
    {
        return await notificationRepository.FindByRecipientAsync(query.TouristId, query.UnreadOnly, cancellationToken);
    }
}
