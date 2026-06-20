using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

public interface IProgressRepository : IBaseRepository<Progress>
{
    Task<IEnumerable<Progress>> FindExperienceByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken);
}
