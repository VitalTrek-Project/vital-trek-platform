using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
/// Personal, travel and health information for a Tourist-role IAM user. Referenced by
/// <c>UserId</c> only (no FK to Iam.User — same cross-context reference convention used
/// throughout the platform, e.g. TourManagement.Tour.AgencyId).
/// </summary>
/// <remarks>
/// Medical fields (<see cref="BloodType"/>, <see cref="Allergies"/>, <see cref="MedicalConditions"/>,
/// <see cref="Medications"/>) and <see cref="EmergencyContacts"/> are sensitive: they must never be
/// serialized in list endpoints, only in an authorized detail read (see ProfilesController and
/// MedicalDataAccessLog).
/// </remarks>
public class TouristProfile
{
    private readonly List<EmergencyContact> _emergencyContacts = [];

    protected TouristProfile()
    {
    }

    public TouristProfile(Guid userId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ProfilesError(ProfilesErrors.InvalidProfileData, "Full name is required.");

        Id = Guid.NewGuid();
        UserId = userId;
        FullName = fullName;
        ExperienceLevel = ExperienceLevel.Beginner;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string? PhotoUrl { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? Nationality { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? PreferredLanguage { get; private set; }
    public ExperienceLevel ExperienceLevel { get; private set; }
    public IdentityDocumentType? IdentityDocumentType { get; private set; }
    public string? IdentityDocumentNumber { get; private set; }
    public BloodType? BloodType { get; private set; }
    public string? Allergies { get; private set; }
    public string? MedicalConditions { get; private set; }
    public string? Medications { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public IReadOnlyCollection<EmergencyContact> EmergencyContacts => _emergencyContacts.AsReadOnly();

    public TouristProfile UpdatePersonalData(
        string fullName,
        string? photoUrl,
        DateOnly? dateOfBirth,
        string? nationality,
        string? phoneNumber,
        string? preferredLanguage,
        ExperienceLevel experienceLevel)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ProfilesError(ProfilesErrors.InvalidProfileData, "Full name is required.");

        FullName = fullName;
        PhotoUrl = photoUrl;
        DateOfBirth = dateOfBirth;
        Nationality = nationality;
        PhoneNumber = phoneNumber;
        PreferredLanguage = preferredLanguage;
        ExperienceLevel = experienceLevel;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public TouristProfile UpdateIdentityDocument(IdentityDocumentType type, string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ProfilesError(ProfilesErrors.InvalidProfileData, "Document number is required.");

        IdentityDocumentType = type;
        IdentityDocumentNumber = number;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public TouristProfile UpdateMedicalInfo(BloodType? bloodType, string? allergies, string? medicalConditions, string? medications)
    {
        BloodType = bloodType;
        Allergies = allergies;
        MedicalConditions = medicalConditions;
        Medications = medications;
        UpdatedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public EmergencyContact AddEmergencyContact(string name, string relationship, string phoneNumber)
    {
        var contact = new EmergencyContact(Id, name, relationship, phoneNumber);
        _emergencyContacts.Add(contact);
        UpdatedAt = DateTimeOffset.UtcNow;
        return contact;
    }

    public void UpdateEmergencyContact(Guid contactId, string name, string relationship, string phoneNumber)
    {
        var contact = _emergencyContacts.FirstOrDefault(c => c.Id == contactId)
                      ?? throw new ProfilesError(ProfilesErrors.EmergencyContactNotFound, "Emergency contact not found.");
        contact.Update(name, relationship, phoneNumber);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RemoveEmergencyContact(Guid contactId)
    {
        var contact = _emergencyContacts.FirstOrDefault(c => c.Id == contactId)
                      ?? throw new ProfilesError(ProfilesErrors.EmergencyContactNotFound, "Emergency contact not found.");
        _emergencyContacts.Remove(contact);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Domain concept of "can this tourist join an expedition?" — requires at least one
    /// emergency contact and a registered identity document. Consumed by TourManagement's
    /// booking flow through <see cref="Interfaces.Acl.IProfilesContextFacade"/>.
    /// </summary>
    public ProfileCompletenessResult EvaluateCompleteness()
    {
        var missing = new List<string>();
        if (_emergencyContacts.Count == 0) missing.Add("emergencyContact");
        if (IdentityDocumentType is null || string.IsNullOrWhiteSpace(IdentityDocumentNumber)) missing.Add("identityDocument");
        if (string.IsNullOrWhiteSpace(PhoneNumber)) missing.Add("phoneNumber");
        if (DateOfBirth is null) missing.Add("dateOfBirth");
        if (string.IsNullOrWhiteSpace(Nationality)) missing.Add("nationality");

        const int totalFields = 5;
        var completionPercentage = (int)Math.Round((totalFields - missing.Count) / (double)totalFields * 100);
        var canJoinExpedition = _emergencyContacts.Count > 0
                                 && IdentityDocumentType is not null
                                 && !string.IsNullOrWhiteSpace(IdentityDocumentNumber);

        return new ProfileCompletenessResult(canJoinExpedition, completionPercentage, missing);
    }
}
