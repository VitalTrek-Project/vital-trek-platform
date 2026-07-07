namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record LoyaltyMetricsResource(
    int PointsIssued,
    int PointsRedeemed,
    int EnrolledTourists,
    IEnumerable<TierDistributionResource> TouristsPerTier,
    IEnumerable<TopTouristResource> TopTourists);

public record TierDistributionResource(Guid? TierId, string TierName, int TouristCount);

public record TopTouristResource(Guid TouristId, int TotalPoints, string? TierName);
