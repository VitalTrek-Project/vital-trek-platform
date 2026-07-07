using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IInAppNotificationRepository : IBaseRepository<InAppNotification>
{
    Task<IReadOnlyList<InAppNotification>> FindByRecipientAsync(Guid touristId, bool? unreadOnly, CancellationToken cancellationToken);

    Task<InAppNotification?> FindByIdAsync(Guid notificationId, CancellationToken cancellationToken);
}
