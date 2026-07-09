using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record LoyaltyProgramResource(
    Guid Id,
    Guid AgencyId,
    int PointsPerExpeditionCompleted,
    int PointsPerExpeditionBooked,
    int PointsPerReferral,
    int PointsPerReview,
    int? ExpirationMonths);

public record UpdateLoyaltyProgramResource(
    [Range(0, int.MaxValue)] int PointsPerExpeditionCompleted,
    [Range(0, int.MaxValue)] int PointsPerExpeditionBooked,
    [Range(0, int.MaxValue)] int PointsPerReferral,
    [Range(0, int.MaxValue)] int PointsPerReview,
    [Range(1, int.MaxValue)] int? ExpirationMonths);

public record LoyaltyTierResource(Guid Id, string Name, int MinPoints, string Benefits, int SortOrder);

public record CreateTierResource(
    [Required] string Name,
    [Range(0, int.MaxValue)] int MinPoints,
    string Benefits,
    int SortOrder);

public record UpdateTierResource(
    [Required] string Name,
    [Range(0, int.MaxValue)] int MinPoints,
    string Benefits,
    int SortOrder);
