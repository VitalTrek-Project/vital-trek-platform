using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

public interface IBinnacleReadingRepository : IBaseRepository<BinnacleReading>
{
    Task<IEnumerable<BinnacleReading>> FindByExpeditionIdAsync(
        int expeditionId,
        CancellationToken cancellationToken);
}
