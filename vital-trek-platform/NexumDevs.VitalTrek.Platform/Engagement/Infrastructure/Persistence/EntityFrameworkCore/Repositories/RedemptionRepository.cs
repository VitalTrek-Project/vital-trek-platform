using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RedemptionRepository(AppDbContext context)
    : BaseRepository<Redemption>(context), IRedemptionRepository
{
    public async Task<IReadOnlyList<Redemption>> FindByTouristAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken)
    {
        return await Context.Set<Redemption>()
            .Where(r => r.AgencyId == agencyId && r.TouristId == touristId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Redemption?> FindByCodeAsync(Guid agencyId, string code, CancellationToken cancellationToken)
    {
        return await Context.Set<Redemption>()
            .FirstOrDefaultAsync(r => r.AgencyId == agencyId && r.Code == code, cancellationToken);
    }

    public async Task<Redemption?> FindByIdAndAgencyAsync(Guid redemptionId, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<Redemption>()
            .FirstOrDefaultAsync(r => r.Id == redemptionId && r.AgencyId == agencyId, cancellationToken);
    }
}
