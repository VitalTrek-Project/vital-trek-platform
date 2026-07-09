using NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Internal.CommandServices;

public class TouristProfileCommandService(ITouristProfileRepository repository, IUnitOfWork unitOfWork)
    : ITouristProfileCommandService
{
    public async Task<TouristProfile> Handle(UpdateTouristProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await repository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (profile is null)
        {
            profile = new TouristProfile(command.UserId, command.FullName);
            await repository.AddAsync(profile, cancellationToken);
        }

        profile.UpdatePersonalData(
            command.FullName,
            command.PhotoUrl,
            command.DateOfBirth,
            command.Nationality,
            command.PhoneNumber,
            command.PreferredLanguage,
            command.ExperienceLevel);

        await unitOfWork.CompleteAsync(cancellationToken);
        return profile;
    }

    public async Task<TouristProfile> Handle(UpdateIdentityDocumentCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetOrThrow(command.UserId, cancellationToken);
        profile.UpdateIdentityDocument(command.Type, command.Number);
        await unitOfWork.CompleteAsync(cancellationToken);
        return profile;
    }

    public async Task<TouristProfile> Handle(UpdateMedicalInfoCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetOrThrow(command.UserId, cancellationToken);
        profile.UpdateMedicalInfo(command.BloodType, command.Allergies, command.MedicalConditions, command.Medications);
        await unitOfWork.CompleteAsync(cancellationToken);
        return profile;
    }

    public async Task<EmergencyContact> Handle(AddEmergencyContactCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetOrThrow(command.UserId, cancellationToken);
        var contact = profile.AddEmergencyContact(command.Name, command.Relationship, command.PhoneNumber);
        await unitOfWork.CompleteAsync(cancellationToken);
        return contact;
    }

    public async Task<EmergencyContact> Handle(UpdateEmergencyContactCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetOrThrow(command.UserId, cancellationToken);
        profile.UpdateEmergencyContact(command.ContactId, command.Name, command.Relationship, command.PhoneNumber);
        await unitOfWork.CompleteAsync(cancellationToken);
        return profile.EmergencyContacts.First(c => c.Id == command.ContactId);
    }

    public async Task Handle(RemoveEmergencyContactCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetOrThrow(command.UserId, cancellationToken);
        profile.RemoveEmergencyContact(command.ContactId);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    private async Task<TouristProfile> GetOrThrow(Guid userId, CancellationToken cancellationToken)
    {
        return await repository.FindByUserIdAsync(userId, cancellationToken)
               ?? throw new Domain.Model.ProfilesError(ProfilesErrors.ProfileNotFound, "Tourist profile not found.");
    }
}
