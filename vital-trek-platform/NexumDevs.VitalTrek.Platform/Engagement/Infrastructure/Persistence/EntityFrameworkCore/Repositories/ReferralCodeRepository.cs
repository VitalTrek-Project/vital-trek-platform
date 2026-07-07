using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReferralCodeRepository(AppDbContext context)
    : BaseRepository<ReferralCode>(context), IReferralCodeRepository
{
    public async Task<ReferralCode?> FindByTouristAndAgencyAsync(Guid touristId, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<ReferralCode>()
            .FirstOrDefaultAsync(c => c.TouristId == touristId && c.AgencyId == agencyId, cancellationToken);
    }

    public async Task<ReferralCode?> FindByCodeAsync(Guid agencyId, string code, CancellationToken cancellationToken)
    {
        return await Context.Set<ReferralCode>()
            .FirstOrDefaultAsync(c => c.AgencyId == agencyId && c.Code == code, cancellationToken);
    }
}
