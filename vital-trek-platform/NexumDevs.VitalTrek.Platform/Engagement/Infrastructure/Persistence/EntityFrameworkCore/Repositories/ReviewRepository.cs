using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReviewRepository(AppDbContext context)
    : BaseRepository<Review>(context), IReviewRepository
{
    public async Task<bool> ExistsAsync(Guid agencyId, Guid touristId, int expeditionId, CancellationToken cancellationToken)
    {
        return await Context.Set<Review>().AnyAsync(
            r => r.AgencyId == agencyId && r.TouristId == touristId && r.ExpeditionId == expeditionId,
            cancellationToken);
    }

    public async Task<int> CountByTouristAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken)
    {
        return await Context.Set<Review>()
            .CountAsync(r => r.AgencyId == agencyId && r.TouristId == touristId, cancellationToken);
    }
}
