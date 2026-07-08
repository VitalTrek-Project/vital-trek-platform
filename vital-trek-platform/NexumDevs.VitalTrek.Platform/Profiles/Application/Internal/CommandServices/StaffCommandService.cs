using NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Internal.CommandServices;

public class StaffCommandService(
    IStaffProfileRepository profileRepository,
    IStaffPreferencesRepository preferencesRepository,
    IUnitOfWork unitOfWork)
    : IStaffCommandService
{
    public async Task<StaffProfile> Handle(UpdateStaffProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (profile is null)
        {
            profile = new StaffProfile(command.UserId, command.AgencyId, command.FullName);
            await profileRepository.AddAsync(profile, cancellationToken);
        }

        profile.UpdatePersonalData(command.FullName, command.PhotoUrl, command.Position, command.ContactPhone);

        await unitOfWork.CompleteAsync(cancellationToken);
        return profile;
    }

    public async Task<StaffPreferences> Handle(UpdateStaffNotificationPreferencesCommand command, CancellationToken cancellationToken)
    {
        var preferences = await preferencesRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (preferences is null)
        {
            preferences = new StaffPreferences(command.UserId);
            await preferencesRepository.AddAsync(preferences, cancellationToken);
        }

        preferences.UpdateNotificationPreferences(command.PendingRedemptionsEnabled, command.NewBookingsEnabled);

        await unitOfWork.CompleteAsync(cancellationToken);
        return preferences;
    }
}
