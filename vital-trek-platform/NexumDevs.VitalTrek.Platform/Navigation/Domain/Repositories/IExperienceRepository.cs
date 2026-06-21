using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

public interface IExperienceRepository : IBaseRepository<Experience>
{
    Task<IEnumerable<Experience>> FindExperienceByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken);
}
