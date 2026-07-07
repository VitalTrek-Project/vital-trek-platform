using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class MetricsResourceAssembler
{
    public static LoyaltyMetricsResource ToResourceFromMetrics(LoyaltyMetrics metrics) => new(
        metrics.PointsIssued,
        metrics.PointsRedeemed,
        metrics.EnrolledTourists,
        metrics.TouristsPerTier.Select(t => new TierDistributionResource(t.TierId, t.TierName, t.TouristCount)),
        metrics.TopTourists.Select(t => new TopTouristResource(t.TouristId, t.TotalPoints, t.TierName)));
}
