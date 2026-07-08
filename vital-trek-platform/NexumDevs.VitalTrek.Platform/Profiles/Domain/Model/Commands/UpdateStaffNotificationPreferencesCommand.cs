namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record UpdateStaffNotificationPreferencesCommand(Guid UserId, bool PendingRedemptionsEnabled, bool NewBookingsEnabled);
