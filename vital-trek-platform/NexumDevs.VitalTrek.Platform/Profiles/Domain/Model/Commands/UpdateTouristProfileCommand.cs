using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

/// <summary>
/// Upserts a tourist's personal/travel data. Creates the profile on first use.
/// </summary>
public record UpdateTouristProfileCommand(
    Guid UserId,
    string FullName,
    string? PhotoUrl,
    DateOnly? DateOfBirth,
    string? Nationality,
    string? PhoneNumber,
    string? PreferredLanguage,
    ExperienceLevel ExperienceLevel);
