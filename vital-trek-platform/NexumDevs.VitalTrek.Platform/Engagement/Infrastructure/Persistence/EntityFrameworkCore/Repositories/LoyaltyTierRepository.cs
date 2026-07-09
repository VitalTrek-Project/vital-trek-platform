using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class LoyaltyTierRepository(AppDbContext context)
    : BaseRepository<LoyaltyTier>(context), ILoyaltyTierRepository
{
    public async Task<IReadOnlyList<LoyaltyTier>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<LoyaltyTier>()
            .Where(t => t.AgencyId == agencyId)
            .OrderBy(t => t.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<LoyaltyTier?> FindByIdAndAgencyAsync(Guid tierId, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<LoyaltyTier>()
            .FirstOrDefaultAsync(t => t.Id == tierId && t.AgencyId == agencyId, cancellationToken);
    }
}
