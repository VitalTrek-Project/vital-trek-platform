using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PointsTransactionRepository(AppDbContext context)
    : BaseRepository<PointsTransaction>(context), IPointsTransactionRepository
{
    public async Task<bool> ExistsAsync(Guid agencyId, Guid touristId, PointsTransactionType type, string sourceId, CancellationToken cancellationToken)
    {
        return await Context.Set<PointsTransaction>().AnyAsync(
            t => t.AgencyId == agencyId && t.TouristId == touristId && t.Type == type && t.SourceId == sourceId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<PointsTransaction>> FindByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        return await Context.Set<PointsTransaction>()
            .Where(t => t.ProfileId == profileId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PointsTransaction>> FindExpiredPendingAsync(Guid profileId, DateTimeOffset asOf, CancellationToken cancellationToken)
    {
        var candidates = await Context.Set<PointsTransaction>()
            .Where(t => t.ProfileId == profileId && t.Points > 0 && t.ExpiresAt != null && t.ExpiresAt <= asOf)
            .ToListAsync(cancellationToken);

        if (candidates.Count == 0) return [];

        var alreadyExpiredSourceIds = await Context.Set<PointsTransaction>()
            .Where(t => t.ProfileId == profileId && t.Type == PointsTransactionType.PointsExpired)
            .Select(t => t.SourceId)
            .ToListAsync(cancellationToken);

        return candidates
            .Where(t => !alreadyExpiredSourceIds.Contains(t.Id.ToString()))
            .ToList();
    }

    public async Task<int> CountByTypeAsync(Guid agencyId, Guid touristId, PointsTransactionType type, CancellationToken cancellationToken)
    {
        return await Context.Set<PointsTransaction>()
            .CountAsync(t => t.AgencyId == agencyId && t.TouristId == touristId && t.Type == type, cancellationToken);
    }

    public async Task<int> SumPointsByTypeAsync(Guid agencyId, IReadOnlyCollection<PointsTransactionType> types, CancellationToken cancellationToken)
    {
        return await Context.Set<PointsTransaction>()
            .Where(t => t.AgencyId == agencyId && types.Contains(t.Type))
            .SumAsync(t => t.Points, cancellationToken);
    }
}
