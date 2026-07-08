using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
/// Expedition, notification and privacy preferences for a Tourist-role IAM user. Kept as
/// its own aggregate (independent lifecycle from <see cref="TouristProfile"/>), the same way
/// Loyalty separates GamificationProfile from LoyaltyProgram.
/// </summary>
public class TouristPreferences
{
    private const char ListSeparator = ';';

    protected TouristPreferences()
    {
    }

    public TouristPreferences(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        // Safety alerts protect tourists in the field and can never be turned off.
        SafetyAlertsEnabled = true;
        LoyaltyUpdatesEnabled = true;
        ExpeditionRemindersEnabled = true;
        ProfileVisibleToExpeditionMates = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    /// <summary>Semicolon-delimited list of preferred activity types. Use <see cref="PreferredActivityTypes"/> to read.</summary>
    public string? PreferredActivityTypesCsv { get; private set; }

    /// <summary>Semicolon-delimited list of dietary restrictions. Use <see cref="DietaryRestrictions"/> to read.</summary>
    public string? DietaryRestrictionsCsv { get; private set; }

    public EDifficultyLevel? PreferredDifficulty { get; private set; }
    public bool SafetyAlertsEnabled { get; private set; }
    public bool LoyaltyUpdatesEnabled { get; private set; }
    public bool ExpeditionRemindersEnabled { get; private set; }
    public bool ProfileVisibleToExpeditionMates { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public IReadOnlyCollection<string> PreferredActivityTypes => Split(PreferredActivityTypesCsv);
    public IReadOnlyCollection<string> DietaryRestrictions => Split(DietaryRestrictionsCsv);

    public TouristPreferences UpdateExpeditionPreferences(
        IEnumerable<string> preferredActivityTypes,
        EDifficultyLevel? preferredDifficulty,
        IEnumerable<string> dietaryRestrictions)
    {
        PreferredActivityTypesCsv = Join(preferredActivityTypes);
        DietaryRestrictionsCsv = Join(dietaryRestrictions);
        PreferredDifficulty = preferredDifficulty;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }

    /// <summary>
    /// Safety alerts are always kept on regardless of the requested value — this is a
    /// safety-platform invariant, not a user preference.
    /// </summary>
    public TouristPreferences UpdateNotificationPreferences(bool loyaltyUpdatesEnabled, bool expeditionRemindersEnabled)
    {
        SafetyAlertsEnabled = true;
        LoyaltyUpdatesEnabled = loyaltyUpdatesEnabled;
        ExpeditionRemindersEnabled = expeditionRemindersEnabled;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public TouristPreferences UpdatePrivacyPreferences(bool profileVisibleToExpeditionMates)
    {
        ProfileVisibleToExpeditionMates = profileVisibleToExpeditionMates;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }

    private static string? Join(IEnumerable<string> values)
    {
        var joined = string.Join(ListSeparator, values.Where(v => !string.IsNullOrWhiteSpace(v)));
        return joined.Length == 0 ? null : joined;
    }

    private static IReadOnlyCollection<string> Split(string? csv) =>
        string.IsNullOrEmpty(csv) ? [] : csv.Split(ListSeparator, StringSplitOptions.RemoveEmptyEntries);
}
