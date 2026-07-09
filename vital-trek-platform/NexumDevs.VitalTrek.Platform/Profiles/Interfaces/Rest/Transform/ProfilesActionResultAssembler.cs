using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Profiles.Resources;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
/// Centralizes the mapping from <see cref="ProfilesError"/> codes to HTTP status codes and
/// ProblemDetails responses, shared by every Profiles controller so each doesn't redefine its
/// own switch/catch block.
/// </summary>
public static class ProfilesActionResultAssembler
{
    private static readonly HashSet<string> NotFoundCodes =
    [
        ProfilesErrors.ProfileNotFound,
        ProfilesErrors.EmergencyContactNotFound,
        ProfilesErrors.PreferencesNotFound,
        ProfilesErrors.StaffProfileNotFound,
        ProfilesErrors.StaffPreferencesNotFound
    ];

    private static readonly HashSet<string> ConflictCodes =
    [
        ProfilesErrors.ProfileAlreadyExists,
        ProfilesErrors.StaffProfileAlreadyExists
    ];

    public static int ResolveStatusCode(string errorCode)
    {
        if (NotFoundCodes.Contains(errorCode)) return StatusCodes.Status404NotFound;
        if (ConflictCodes.Contains(errorCode)) return StatusCodes.Status409Conflict;
        if (errorCode == ProfilesErrors.UnauthorizedProfileAccess) return StatusCodes.Status403Forbidden;
        return StatusCodes.Status400BadRequest;
    }

    public static IActionResult ToProblemDetails(
        ControllerBase controller,
        ProfilesError error,
        IStringLocalizer<ProfilesMessages> localizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        var statusCode = ResolveStatusCode(error.ErrorCode);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, (Enum?)null, localizer[error.ErrorCode]);
    }
}
