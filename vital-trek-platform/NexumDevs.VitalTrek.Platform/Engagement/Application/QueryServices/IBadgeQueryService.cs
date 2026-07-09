using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface IBadgeQueryService
{
    Task<IReadOnlyList<BadgeDefinition>> Handle(GetBadgeCatalogQuery query, CancellationToken cancellationToken);
}
