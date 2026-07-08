using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MedicalDataAccessLogRepository(AppDbContext context)
    : BaseRepository<MedicalDataAccessLog>(context), IMedicalDataAccessLogRepository
{
    public async Task<IReadOnlyList<MedicalDataAccessLog>> FindByTouristProfileIdAsync(Guid touristProfileId, CancellationToken cancellationToken)
    {
        return await Context.Set<MedicalDataAccessLog>()
            .Where(l => l.TouristProfileId == touristProfileId)
            .OrderByDescending(l => l.AccessedAt)
            .ToListAsync(cancellationToken);
    }
}
