using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Transform;

public static class StaffResourceFromEntityAssembler
{
    public static StaffProfileResource ToResourceFromEntity(StaffProfile profile) => new(
        profile.Id, profile.UserId, profile.AgencyId, profile.FullName, profile.PhotoUrl, profile.Position, profile.ContactPhone);

    public static StaffPreferencesResource ToResourceFromEntity(StaffPreferences preferences) => new(
        preferences.Id, preferences.UserId, preferences.CriticalAlertsEnabled, preferences.PendingRedemptionsEnabled, preferences.NewBookingsEnabled);
}
