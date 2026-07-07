using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record LoyaltyProfileResource(
    Guid TouristId,
    Guid AgencyId,
    int TotalPoints,
    Guid? CurrentTierId,
    string? CurrentTierName,
    string? NextTierName,
    int? PointsToNextTier);

public record PointsTransactionResource(
    Guid Id,
    string Type,
    int Points,
    string? SourceId,
    string Description,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset CreatedAt);

/// <summary>
/// <see cref="Type"/> must be one of ExpeditionCompleted, ExpeditionBooked, ReferralBonus,
/// ReviewSubmitted — the only events the backend resolves a per-action point value for.
/// The caller never supplies a point amount.
/// </summary>
public record RecordPointsEventResource([Required] string Type, string? SourceId);

public record AwardedBadgeResource(Guid Id, Guid BadgeDefinitionId, string Code, string Name, string Description, DateTimeOffset AwardedAt);

public record ReferralCodeResource(string Code);

public record SubmitReviewResource(
    [Required] int ExpeditionId,
    [Range(1, 5)] int Rating,
    string? Comment);

public record ReviewResource(Guid Id, int ExpeditionId, int Rating, string? Comment, DateTimeOffset CreatedAt);
