using NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Internal.QueryServices;

public class TouristPreferencesQueryService(ITouristPreferencesRepository repository) : ITouristPreferencesQueryService
{
    public async Task<TouristPreferences?> Handle(GetTouristPreferencesByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.FindByUserIdAsync(query.UserId, cancellationToken);
    }
}
