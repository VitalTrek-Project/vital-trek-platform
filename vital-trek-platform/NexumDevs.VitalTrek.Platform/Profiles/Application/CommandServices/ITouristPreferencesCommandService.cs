using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;

public interface ITouristPreferencesCommandService
{
    Task<TouristPreferences> Handle(UpdateExpeditionPreferencesCommand command, CancellationToken cancellationToken);
    Task<TouristPreferences> Handle(UpdateTouristNotificationPreferencesCommand command, CancellationToken cancellationToken);
    Task<TouristPreferences> Handle(UpdatePrivacyPreferencesCommand command, CancellationToken cancellationToken);
}
