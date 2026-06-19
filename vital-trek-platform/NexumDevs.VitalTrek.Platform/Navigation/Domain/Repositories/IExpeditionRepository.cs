using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

public interface IExpeditionRepository : IBaseRepository<Expedition>
{
    Task<bool> ExistsByExpeditionNameAsync(string expeditionName, CancellationToken cancellationToken);
}
