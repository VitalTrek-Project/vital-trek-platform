using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;

public interface IStaffCommandService
{
    Task<StaffProfile> Handle(UpdateStaffProfileCommand command, CancellationToken cancellationToken);
    Task<StaffPreferences> Handle(UpdateStaffNotificationPreferencesCommand command, CancellationToken cancellationToken);
}
