namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record UpdateTouristNotificationPreferencesCommand(Guid UserId, bool LoyaltyUpdatesEnabled, bool ExpeditionRemindersEnabled);
