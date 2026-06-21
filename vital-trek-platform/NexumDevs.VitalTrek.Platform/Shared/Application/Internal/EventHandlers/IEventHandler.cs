using NexumDevs.VitalTrek.Platform.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace NexumDevs.VitalTrek.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent, INotification
{
}
