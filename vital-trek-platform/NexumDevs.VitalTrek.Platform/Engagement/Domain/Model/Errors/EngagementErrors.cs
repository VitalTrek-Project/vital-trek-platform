namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

/// <summary>
/// Centralized catalog of error keys used by the Engagement bounded context.
/// These keys are intended to be used together with <c>IStringLocalizer</c>
/// to retrieve localized error messages from resource files.
/// </summary>
public static class EngagementErrors
{
    /// <summary>
    /// Error raised when the points value is invalid (zero or negative).
    /// </summary>
    public const string InvalidPoints = "InvalidPoints";

    /// <summary>
    /// Error raised when the specified expedition has already been awarded to the profile.
    /// </summary>
    public const string ExpeditionAlreadyAwarded = "ExpeditionAlreadyAwarded";

    /// <summary>
    /// Error raised when the gamification profile cannot be found.
    /// </summary>
    public const string ProfileNotFound = "ProfileNotFound";
}
