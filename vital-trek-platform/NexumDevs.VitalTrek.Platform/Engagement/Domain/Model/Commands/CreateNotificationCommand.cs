using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

/// <summary>Internal command — raised by other Engagement services when a notifiable event occurs.</summary>
public record CreateNotificationCommand(Guid RecipientTouristId, Guid? AgencyId, NotificationType Type, string Title, string Message);
