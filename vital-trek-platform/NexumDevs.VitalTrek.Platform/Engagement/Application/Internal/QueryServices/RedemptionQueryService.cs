using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class RedemptionQueryService(IRedemptionRepository redemptionRepository) : IRedemptionQueryService
{
    public async Task<IReadOnlyList<Redemption>> Handle(GetRedemptionsForTouristQuery query, CancellationToken cancellationToken)
    {
        return await redemptionRepository.FindByTouristAsync(query.AgencyId, query.TouristId, cancellationToken);
    }

    public async Task<Redemption?> Handle(FindRedemptionByCodeQuery query, CancellationToken cancellationToken)
    {
        return await redemptionRepository.FindByCodeAsync(query.AgencyId, query.Code, cancellationToken);
    }
}
