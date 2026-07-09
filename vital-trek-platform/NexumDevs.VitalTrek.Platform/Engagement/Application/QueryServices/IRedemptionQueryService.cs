using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface IRedemptionQueryService
{
    Task<IReadOnlyList<Redemption>> Handle(GetRedemptionsForTouristQuery query, CancellationToken cancellationToken);
    Task<Redemption?> Handle(FindRedemptionByCodeQuery query, CancellationToken cancellationToken);
}
