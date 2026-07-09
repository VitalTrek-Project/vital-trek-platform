using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root for a simple in-app notification feed. Lives in this bounded
/// context for now since Engagement is its only producer; if another context needs
/// notifications later this can move to Shared without changing its shape.
/// </summary>
public class InAppNotification
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected InAppNotification() { }

    public InAppNotification(Guid recipientTouristId, Guid? agencyId, NotificationType type, string title, string message)
    {
        Id = Guid.NewGuid();
        RecipientTouristId = recipientTouristId;
        AgencyId = agencyId;
        Type = type;
        Title = title;
        Message = message;
        IsRead = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid RecipientTouristId { get; private set; }
    public Guid? AgencyId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
