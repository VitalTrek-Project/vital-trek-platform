using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;

public interface IVitalSignReadingRepository : IBaseRepository<VitalSignReading>
{
    Task<IEnumerable<VitalSignReading>> FindByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken);
    Task<VitalSignReading?> FindLatestByTouristAsync(int touristId, int expeditionId, CancellationToken cancellationToken);
}