using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;

public interface ITouristPreferencesQueryService
{
    Task<TouristPreferences?> Handle(GetTouristPreferencesByUserIdQuery query, CancellationToken cancellationToken);
}
