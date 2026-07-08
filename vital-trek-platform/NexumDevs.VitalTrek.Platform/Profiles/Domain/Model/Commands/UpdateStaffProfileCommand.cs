namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

/// <summary>
/// Upserts an agency staff member's profile. Creates it on first use.
/// </summary>
public record UpdateStaffProfileCommand(Guid UserId, Guid AgencyId, string FullName, string? PhotoUrl, string? Position, string? ContactPhone);
