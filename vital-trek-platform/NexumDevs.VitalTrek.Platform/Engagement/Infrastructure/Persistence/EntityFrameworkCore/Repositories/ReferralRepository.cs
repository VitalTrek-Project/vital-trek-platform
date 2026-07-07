using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReferralRepository(AppDbContext context)
    : BaseRepository<Referral>(context), IReferralRepository
{
    public async Task<Referral?> FindPendingByReferredTouristAsync(Guid agencyId, Guid referredTouristId, CancellationToken cancellationToken)
    {
        return await Context.Set<Referral>()
            .FirstOrDefaultAsync(
                r => r.AgencyId == agencyId && r.ReferredTouristId == referredTouristId && r.Status == ReferralStatus.Pending,
                cancellationToken);
    }

    public async Task<int> CountCompletedByReferrerAsync(Guid agencyId, Guid referrerTouristId, CancellationToken cancellationToken)
    {
        return await Context.Set<Referral>()
            .CountAsync(
                r => r.AgencyId == agencyId && r.ReferrerTouristId == referrerTouristId && r.Status == ReferralStatus.Completed,
                cancellationToken);
    }
}
