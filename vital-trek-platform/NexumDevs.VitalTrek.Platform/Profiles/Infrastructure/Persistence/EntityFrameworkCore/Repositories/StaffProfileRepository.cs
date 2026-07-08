using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StaffProfileRepository(AppDbContext context)
    : BaseRepository<StaffProfile>(context), IStaffProfileRepository
{
    public async Task<StaffProfile?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<StaffProfile>().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }
}
