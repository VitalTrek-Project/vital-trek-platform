using NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Internal.CommandServices;

public class TouristPreferencesCommandService(ITouristPreferencesRepository repository, IUnitOfWork unitOfWork)
    : ITouristPreferencesCommandService
{
    public async Task<TouristPreferences> Handle(UpdateExpeditionPreferencesCommand command, CancellationToken cancellationToken)
    {
        var preferences = await GetOrCreate(command.UserId, cancellationToken);
        preferences.UpdateExpeditionPreferences(command.PreferredActivityTypes, command.PreferredDifficulty, command.DietaryRestrictions);
        await unitOfWork.CompleteAsync(cancellationToken);
        return preferences;
    }

    public async Task<TouristPreferences> Handle(UpdateTouristNotificationPreferencesCommand command, CancellationToken cancellationToken)
    {
        var preferences = await GetOrCreate(command.UserId, cancellationToken);
        preferences.UpdateNotificationPreferences(command.LoyaltyUpdatesEnabled, command.ExpeditionRemindersEnabled);
        await unitOfWork.CompleteAsync(cancellationToken);
        return preferences;
    }

    public async Task<TouristPreferences> Handle(UpdatePrivacyPreferencesCommand command, CancellationToken cancellationToken)
    {
        var preferences = await GetOrCreate(command.UserId, cancellationToken);
        preferences.UpdatePrivacyPreferences(command.ProfileVisibleToExpeditionMates);
        await unitOfWork.CompleteAsync(cancellationToken);
        return preferences;
    }

    private async Task<TouristPreferences> GetOrCreate(Guid userId, CancellationToken cancellationToken)
    {
        var preferences = await repository.FindByUserIdAsync(userId, cancellationToken);
        if (preferences is not null) return preferences;

        preferences = new TouristPreferences(userId);
        await repository.AddAsync(preferences, cancellationToken);
        return preferences;
    }
}
