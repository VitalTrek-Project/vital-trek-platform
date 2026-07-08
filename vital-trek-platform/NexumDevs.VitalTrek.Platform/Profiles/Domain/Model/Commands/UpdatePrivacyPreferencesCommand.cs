namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record UpdatePrivacyPreferencesCommand(Guid UserId, bool ProfileVisibleToExpeditionMates);
