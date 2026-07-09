using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;

public interface ITouristPreferencesRepository : IBaseRepository<TouristPreferences>
{
    Task<TouristPreferences?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
