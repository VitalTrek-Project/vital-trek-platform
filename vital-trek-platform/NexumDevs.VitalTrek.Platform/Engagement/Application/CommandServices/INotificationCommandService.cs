using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface INotificationCommandService
{
    Task<InAppNotification> Handle(CreateNotificationCommand command, CancellationToken cancellationToken);
    Task Handle(MarkNotificationReadCommand command, CancellationToken cancellationToken);
}
