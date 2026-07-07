using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IBadgeDefinitionRepository : IBaseRepository<BadgeDefinition>
{
    /// <summary>Global (AgencyId == null) definitions plus, when provided, one agency's custom ones.</summary>
    Task<IReadOnlyList<BadgeDefinition>> FindCatalogAsync(Guid? agencyId, CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);

    Task<BadgeDefinition?> FindByIdAsync(Guid badgeDefinitionId, CancellationToken cancellationToken);
}
