using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class BadgeQueryService(IBadgeDefinitionRepository badgeDefinitionRepository) : IBadgeQueryService
{
    public async Task<IReadOnlyList<BadgeDefinition>> Handle(GetBadgeCatalogQuery query, CancellationToken cancellationToken)
    {
        return await badgeDefinitionRepository.FindCatalogAsync(query.AgencyId, cancellationToken);
    }
}
