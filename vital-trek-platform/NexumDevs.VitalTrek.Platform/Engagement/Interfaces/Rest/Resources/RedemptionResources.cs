using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record RedemptionResource(
    Guid Id,
    Guid TouristId,
    Guid RewardId,
    int PointsSpent,
    string Code,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UsedAt,
    DateTimeOffset? ExpiresAt);

public record RedeemRewardResource([Required] Guid RewardId);

public record MarkRedemptionUsedResource([Required] string Status);
