using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StaffPreferencesRepository(AppDbContext context)
    : BaseRepository<StaffPreferences>(context), IStaffPreferencesRepository
{
    public async Task<StaffPreferences?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<StaffPreferences>().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }
}
