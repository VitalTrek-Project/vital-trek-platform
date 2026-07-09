namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ReadModels;

/// <summary>Aggregated read model backing the admin dashboard's loyalty cards.</summary>
public record LoyaltyMetrics(
    int PointsIssued,
    int PointsRedeemed,
    int EnrolledTourists,
    IReadOnlyList<TierDistribution> TouristsPerTier,
    IReadOnlyList<TopTourist> TopTourists);

public record TierDistribution(Guid? TierId, string TierName, int TouristCount);

public record TopTourist(Guid TouristId, int TotalPoints, string? TierName);
