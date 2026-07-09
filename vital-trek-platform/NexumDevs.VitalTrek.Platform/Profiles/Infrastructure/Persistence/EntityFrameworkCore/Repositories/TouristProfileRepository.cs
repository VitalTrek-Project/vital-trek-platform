using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TouristProfileRepository(AppDbContext context)
    : BaseRepository<TouristProfile>(context), ITouristProfileRepository
{
    public async Task<TouristProfile?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<TouristProfile>()
            .Include(p => p.EmergencyContacts)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }
}
