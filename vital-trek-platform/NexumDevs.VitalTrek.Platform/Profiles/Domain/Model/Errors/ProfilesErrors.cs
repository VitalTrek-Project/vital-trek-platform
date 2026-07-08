namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;

/// <summary>
/// Centralized catalog of error keys used by the Profiles bounded context.
/// These keys are intended to be used together with <c>IStringLocalizer</c>
/// to retrieve localized error messages from resource files.
/// </summary>
public static class ProfilesErrors
{
    public const string ProfileNotFound = "ProfileNotFound";
    public const string ProfileAlreadyExists = "ProfileAlreadyExists";
    public const string InvalidProfileData = "InvalidProfileData";
    public const string EmergencyContactNotFound = "EmergencyContactNotFound";
    public const string InvalidEmergencyContactData = "InvalidEmergencyContactData";
    public const string PreferencesNotFound = "PreferencesNotFound";
    public const string StaffProfileNotFound = "StaffProfileNotFound";
    public const string StaffProfileAlreadyExists = "StaffProfileAlreadyExists";
    public const string StaffPreferencesNotFound = "StaffPreferencesNotFound";
    public const string UnauthorizedProfileAccess = "UnauthorizedProfileAccess";
}
