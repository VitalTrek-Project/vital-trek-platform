using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

public class NotificationCommandService(
    IInAppNotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : INotificationCommandService
{
    public async Task<InAppNotification> Handle(CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        var notification = new InAppNotification(command.RecipientTouristId, command.AgencyId, command.Type, command.Title, command.Message);
        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return notification;
    }

    public async Task Handle(MarkNotificationReadCommand command, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.FindByIdAsync(command.NotificationId, cancellationToken)
                            ?? throw new EngagementError(EngagementErrors.NotificationNotFound);

        notification.MarkAsRead();
        notificationRepository.Update(notification);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
