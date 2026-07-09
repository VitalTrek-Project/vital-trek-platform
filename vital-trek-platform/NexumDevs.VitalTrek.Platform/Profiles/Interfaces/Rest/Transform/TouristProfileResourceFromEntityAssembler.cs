using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Transform;

public static class TouristProfileResourceFromEntityAssembler
{
    public static TouristProfileResource ToResourceFromEntity(TouristProfile profile) => new(
        profile.Id,
        profile.UserId,
        profile.FullName,
        profile.PhotoUrl,
        profile.DateOfBirth,
        profile.Nationality,
        profile.PhoneNumber,
        profile.PreferredLanguage,
        profile.ExperienceLevel.ToString(),
        profile.IdentityDocumentType?.ToString(),
        profile.IdentityDocumentNumber,
        profile.BloodType?.ToString(),
        profile.Allergies,
        profile.MedicalConditions,
        profile.Medications,
        profile.EmergencyContacts.Select(c => new EmergencyContactResource(c.Id, c.Name, c.Relationship, c.PhoneNumber)));
}
