using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;

public interface ILocationReadingRepository : IBaseRepository<LocationReading>
{
    Task<IEnumerable<LocationReading>> FindByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken);
    Task<LocationReading?> FindLatestByTouristAsync(int touristId, int expeditionId, CancellationToken cancellationToken);
}