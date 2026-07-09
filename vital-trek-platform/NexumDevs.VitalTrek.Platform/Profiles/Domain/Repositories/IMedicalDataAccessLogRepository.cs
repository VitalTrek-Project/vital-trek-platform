using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;

public interface IMedicalDataAccessLogRepository : IBaseRepository<MedicalDataAccessLog>
{
    Task<IReadOnlyList<MedicalDataAccessLog>> FindByTouristProfileIdAsync(Guid touristProfileId, CancellationToken cancellationToken);
}
