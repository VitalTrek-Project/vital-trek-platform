using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RewardRepository(AppDbContext context)
    : BaseRepository<Reward>(context), IRewardRepository
{
    public async Task<IReadOnlyList<Reward>> FindByAgencyIdAsync(Guid agencyId, bool? activeOnly, CancellationToken cancellationToken)
    {
        var query = Context.Set<Reward>().Where(r => r.AgencyId == agencyId);
        if (activeOnly == true) query = query.Where(r => r.IsActive);
        return await query.OrderBy(r => r.PointsCost).ToListAsync(cancellationToken);
    }

    public async Task<Reward?> FindByIdAndAgencyAsync(Guid rewardId, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<Reward>()
            .FirstOrDefaultAsync(r => r.Id == rewardId && r.AgencyId == agencyId, cancellationToken);
    }
}
