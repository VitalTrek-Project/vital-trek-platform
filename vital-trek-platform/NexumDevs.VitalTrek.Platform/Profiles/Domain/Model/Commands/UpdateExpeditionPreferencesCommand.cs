using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

/// <summary>
/// Upserts a tourist's expedition preferences. Creates the preferences (with default
/// notification/privacy settings) on first use.
/// </summary>
public record UpdateExpeditionPreferencesCommand(
    Guid UserId,
    IReadOnlyList<string> PreferredActivityTypes,
    EDifficultyLevel? PreferredDifficulty,
    IReadOnlyList<string> DietaryRestrictions);
