using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class LoyaltyProgramRepository(AppDbContext context)
    : BaseRepository<LoyaltyProgram>(context), ILoyaltyProgramRepository
{
    public async Task<LoyaltyProgram?> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<LoyaltyProgram>()
            .FirstOrDefaultAsync(p => p.AgencyId == agencyId, cancellationToken);
    }
}
