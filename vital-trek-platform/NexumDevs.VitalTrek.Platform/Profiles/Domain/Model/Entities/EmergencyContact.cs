using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Entities;

/// <summary>
/// A contact to reach in case of emergency during an expedition. Owned by a
/// <see cref="Aggregates.TouristProfile"/> — a tourist needs at least one to be
/// eligible to join an expedition (see <see cref="Aggregates.TouristProfile.EvaluateCompleteness"/>).
/// </summary>
public class EmergencyContact
{
    protected EmergencyContact() { }

    public EmergencyContact(Guid touristProfileId, string name, string relationship, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber))
            throw new ProfilesError(ProfilesErrors.InvalidEmergencyContactData,
                "Name and phone number are required for an emergency contact.");

        Id = Guid.NewGuid();
        TouristProfileId = touristProfileId;
        Name = name;
        Relationship = relationship;
        PhoneNumber = phoneNumber;
    }

    public Guid Id { get; private set; }
    public Guid TouristProfileId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Relationship { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public EmergencyContact Update(string name, string relationship, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber))
            throw new ProfilesError(ProfilesErrors.InvalidEmergencyContactData,
                "Name and phone number are required for an emergency contact.");

        Name = name;
        Relationship = relationship;
        PhoneNumber = phoneNumber;
        return this;
    }
}
