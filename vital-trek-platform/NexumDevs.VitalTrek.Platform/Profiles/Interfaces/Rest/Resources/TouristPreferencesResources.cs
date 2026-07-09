namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;

public record TouristPreferencesResource(
    Guid Id,
    Guid UserId,
    IEnumerable<string> PreferredActivityTypes,
    string? PreferredDifficulty,
    IEnumerable<string> DietaryRestrictions,
    bool SafetyAlertsEnabled,
    bool LoyaltyUpdatesEnabled,
    bool ExpeditionRemindersEnabled,
    bool ProfileVisibleToExpeditionMates);

public record UpdateExpeditionPreferencesResource(
    IEnumerable<string> PreferredActivityTypes,
    string? PreferredDifficulty,
    IEnumerable<string> DietaryRestrictions);

public record UpdateTouristNotificationPreferencesResource(bool LoyaltyUpdatesEnabled, bool ExpeditionRemindersEnabled);

public record UpdatePrivacyPreferencesResource(bool ProfileVisibleToExpeditionMates);
