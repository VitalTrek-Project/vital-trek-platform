using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ReadModels;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface IMetricsQueryService
{
    Task<LoyaltyMetrics> Handle(GetLoyaltyMetricsQuery query, CancellationToken cancellationToken);
}
