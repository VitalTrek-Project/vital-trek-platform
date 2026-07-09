using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<bool> ExistsAsync(Guid agencyId, Guid touristId, int expeditionId, CancellationToken cancellationToken);

    Task<int> CountByTouristAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken);
}
