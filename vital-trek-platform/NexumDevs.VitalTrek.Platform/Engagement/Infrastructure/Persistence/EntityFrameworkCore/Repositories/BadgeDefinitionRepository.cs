using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BadgeDefinitionRepository(AppDbContext context)
    : BaseRepository<BadgeDefinition>(context), IBadgeDefinitionRepository
{
    public async Task<IReadOnlyList<BadgeDefinition>> FindCatalogAsync(Guid? agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<BadgeDefinition>()
            .Where(b => b.AgencyId == null || b.AgencyId == agencyId)
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await Context.Set<BadgeDefinition>().AnyAsync(b => b.Code == code, cancellationToken);
    }

    public async Task<BadgeDefinition?> FindByIdAsync(Guid badgeDefinitionId, CancellationToken cancellationToken)
    {
        return await Context.Set<BadgeDefinition>()
            .FirstOrDefaultAsync(b => b.Id == badgeDefinitionId, cancellationToken);
    }
}
