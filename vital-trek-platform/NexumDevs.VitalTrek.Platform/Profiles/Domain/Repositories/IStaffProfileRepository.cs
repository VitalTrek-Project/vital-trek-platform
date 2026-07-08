using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;

public interface IStaffProfileRepository : IBaseRepository<StaffProfile>
{
    Task<StaffProfile?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
