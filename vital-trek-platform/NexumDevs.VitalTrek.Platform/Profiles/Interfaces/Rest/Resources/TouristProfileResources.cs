using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
/// Full tourist profile detail, including sensitive medical/emergency data. Only ever
/// returned from a detail endpoint (self-read or agency-authorized read) — never from a
/// list endpoint.
/// </summary>
public record TouristProfileResource(
    Guid Id,
    Guid UserId,
    string FullName,
    string? PhotoUrl,
    DateOnly? DateOfBirth,
    string? Nationality,
    string? PhoneNumber,
    string? PreferredLanguage,
    string ExperienceLevel,
    string? IdentityDocumentType,
    string? IdentityDocumentNumber,
    string? BloodType,
    string? Allergies,
    string? MedicalConditions,
    string? Medications,
    IEnumerable<EmergencyContactResource> EmergencyContacts);

public record EmergencyContactResource(Guid Id, string Name, string Relationship, string PhoneNumber);

public record UpdateTouristProfileResource(
    [Required] string FullName,
    string? PhotoUrl,
    DateOnly? DateOfBirth,
    string? Nationality,
    string? PhoneNumber,
    string? PreferredLanguage,
    [Required] string ExperienceLevel);

public record UpdateIdentityDocumentResource([Required] string Type, [Required] string Number);

public record UpdateMedicalInfoResource(string? BloodType, string? Allergies, string? MedicalConditions, string? Medications);

public record EmergencyContactRequestResource([Required] string Name, string Relationship, [Required] string PhoneNumber);

public record ProfileCompletenessResource(bool CanJoinExpedition, int CompletionPercentage, IEnumerable<string> MissingFields);

public record MedicalDataAccessLogResource(Guid Id, Guid AccessedByStaffUserId, DateTimeOffset AccessedAt);
