namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

/// <summary>
/// Centralized catalog of error keys used by the Engagement (Loyalty) bounded context.
/// These keys are intended to be used together with <c>IStringLocalizer</c>
/// to retrieve localized error messages from resource files.
/// </summary>
public static class EngagementErrors
{
    public const string InvalidPoints = "InvalidPoints";
    public const string DuplicateTransaction = "DuplicateTransaction";
    public const string ProfileNotFound = "ProfileNotFound";
    public const string ProgramNotFound = "ProgramNotFound";
    public const string InvalidProgramConfiguration = "InvalidProgramConfiguration";
    public const string TierNotFound = "TierNotFound";
    public const string InvalidTierConfiguration = "InvalidTierConfiguration";
    public const string RewardNotFound = "RewardNotFound";
    public const string RewardInactive = "RewardInactive";
    public const string InsufficientStock = "InsufficientStock";
    public const string InsufficientBalance = "InsufficientBalance";
    public const string RedemptionNotFound = "RedemptionNotFound";
    public const string RedemptionAlreadyProcessed = "RedemptionAlreadyProcessed";
    public const string ReferralCodeNotFound = "ReferralCodeNotFound";
    public const string SelfReferralNotAllowed = "SelfReferralNotAllowed";
    public const string ReferralAlreadyUsed = "ReferralAlreadyUsed";
    public const string BadgeDefinitionNotFound = "BadgeDefinitionNotFound";
    public const string DuplicateBadgeCode = "DuplicateBadgeCode";
    public const string InvalidReview = "InvalidReview";
    public const string DuplicateReview = "DuplicateReview";
    public const string NotificationNotFound = "NotificationNotFound";
    public const string InvalidEventType = "InvalidEventType";
    public const string ConcurrentModification = "ConcurrentModification";
}
