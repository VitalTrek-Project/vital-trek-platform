using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
/// Basic profile information for an Agency-role IAM user (agency staff/admin).
/// </summary>
public class StaffProfile
{
    protected StaffProfile()
    {
    }

    public StaffProfile(Guid userId, Guid agencyId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ProfilesError(ProfilesErrors.InvalidProfileData, "Full name is required.");

        Id = Guid.NewGuid();
        UserId = userId;
        AgencyId = agencyId;
        FullName = fullName;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AgencyId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string? PhotoUrl { get; private set; }
    public string? Position { get; private set; }
    public string? ContactPhone { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public StaffProfile UpdatePersonalData(string fullName, string? photoUrl, string? position, string? contactPhone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ProfilesError(ProfilesErrors.InvalidProfileData, "Full name is required.");

        FullName = fullName;
        PhotoUrl = photoUrl;
        Position = position;
        ContactPhone = contactPhone;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }
}
