using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface INotificationQueryService
{
    Task<IReadOnlyList<InAppNotification>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken);
}
