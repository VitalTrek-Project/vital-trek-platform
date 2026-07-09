namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
/// Notification preferences for an Agency-role IAM user (agency staff/admin).
/// </summary>
public class StaffPreferences
{
    protected StaffPreferences()
    {
    }

    public StaffPreferences(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        // Critical safety alerts can never be turned off.
        CriticalAlertsEnabled = true;
        PendingRedemptionsEnabled = true;
        NewBookingsEnabled = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public bool CriticalAlertsEnabled { get; private set; }
    public bool PendingRedemptionsEnabled { get; private set; }
    public bool NewBookingsEnabled { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Critical alerts are always kept on regardless of the requested value — this is a
    /// safety-platform invariant, not a user preference.
    /// </summary>
    public StaffPreferences UpdateNotificationPreferences(bool pendingRedemptionsEnabled, bool newBookingsEnabled)
    {
        CriticalAlertsEnabled = true;
        PendingRedemptionsEnabled = pendingRedemptionsEnabled;
        NewBookingsEnabled = newBookingsEnabled;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }
}
