using Microsoft.AspNetCore.Http;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

/// <summary>
/// Centralizes the mapping from <see cref="EngagementError"/> codes to HTTP status
/// codes, shared by every Loyalty controller so each doesn't redefine its own switch.
/// </summary>
public static class EngagementActionResultAssembler
{
    private static readonly HashSet<string> NotFoundCodes =
    [
        EngagementErrors.ProfileNotFound,
        EngagementErrors.ProgramNotFound,
        EngagementErrors.TierNotFound,
        EngagementErrors.RewardNotFound,
        EngagementErrors.RedemptionNotFound,
        EngagementErrors.ReferralCodeNotFound,
        EngagementErrors.BadgeDefinitionNotFound,
        EngagementErrors.NotificationNotFound
    ];

    private static readonly HashSet<string> ConflictCodes =
    [
        EngagementErrors.DuplicateTransaction,
        EngagementErrors.DuplicateReview,
        EngagementErrors.DuplicateBadgeCode,
        EngagementErrors.ReferralAlreadyUsed,
        EngagementErrors.RedemptionAlreadyProcessed,
        EngagementErrors.InsufficientBalance,
        EngagementErrors.InsufficientStock,
        EngagementErrors.RewardInactive,
        EngagementErrors.SelfReferralNotAllowed,
        EngagementErrors.ConcurrentModification
    ];

    public static int ResolveStatusCode(EngagementError error)
    {
        if (NotFoundCodes.Contains(error.ErrorCode)) return StatusCodes.Status404NotFound;
        if (ConflictCodes.Contains(error.ErrorCode)) return StatusCodes.Status409Conflict;
        return StatusCodes.Status400BadRequest;
    }
}
