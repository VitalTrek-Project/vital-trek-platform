using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;

public interface IStaffPreferencesRepository : IBaseRepository<StaffPreferences>
{
    Task<StaffPreferences?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
