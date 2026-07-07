using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IPointsTransactionRepository : IBaseRepository<PointsTransaction>
{
    Task<bool> ExistsAsync(Guid agencyId, Guid touristId, PointsTransactionType type, string sourceId, CancellationToken cancellationToken);

    Task<IReadOnlyList<PointsTransaction>> FindByProfileAsync(Guid profileId, CancellationToken cancellationToken);

    /// <summary>
    /// Earning entries for the profile whose <c>ExpiresAt</c> has passed and that do not
    /// yet have a corresponding <see cref="PointsTransactionType.PointsExpired"/> entry
    /// referencing them by <c>SourceId</c>.
    /// </summary>
    Task<IReadOnlyList<PointsTransaction>> FindExpiredPendingAsync(Guid profileId, DateTimeOffset asOf, CancellationToken cancellationToken);

    Task<int> CountByTypeAsync(Guid agencyId, Guid touristId, PointsTransactionType type, CancellationToken cancellationToken);

    Task<int> SumPointsByTypeAsync(Guid agencyId, IReadOnlyCollection<PointsTransactionType> types, CancellationToken cancellationToken);
}
