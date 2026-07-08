using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Profiles.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest;

/// <summary>
/// An agency staff member's own profile and notification preferences — much smaller surface
/// than the tourist side, so profile and preferences are handled by a single controller.
/// </summary>
[ApiController]
[Route("api/v1/profiles/agencies/{agencyId:guid}/staff/{staffUserId:guid}")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available staff profile endpoints")]
public class StaffProfilesController(
    IStaffCommandService commandService,
    IStaffQueryService queryService,
    IStringLocalizer<ProfilesMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get a staff member's own profile", OperationId = "GetStaffProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was found", typeof(StaffProfileResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No profile exists yet for this staff member")]
    public async Task<IActionResult> GetProfile([FromRoute] Guid staffUserId, CancellationToken cancellationToken)
    {
        var profile = await queryService.Handle(new GetStaffProfileByUserIdQuery(staffUserId), cancellationToken);
        if (profile is null)
            return ProfilesActionResultAssembler.ToProblemDetails(
                this, new ProfilesError(ProfilesErrors.StaffProfileNotFound), localizer, problemDetailsFactory);

        return Ok(StaffResourceFromEntityAssembler.ToResourceFromEntity(profile));
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Create or update a staff member's own profile", OperationId = "UpdateStaffProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was saved", typeof(StaffProfileResource))]
    public async Task<IActionResult> UpdateProfile(
        [FromRoute] Guid agencyId, [FromRoute] Guid staffUserId, [FromBody] UpdateStaffProfileResource resource,
        CancellationToken cancellationToken)
    {
        var profile = await commandService.Handle(
            new UpdateStaffProfileCommand(staffUserId, agencyId, resource.FullName, resource.PhotoUrl, resource.Position, resource.ContactPhone),
            cancellationToken);
        return Ok(StaffResourceFromEntityAssembler.ToResourceFromEntity(profile));
    }

    [HttpGet("preferences")]
    [SwaggerOperation(Summary = "Get a staff member's notification preferences", OperationId = "GetStaffPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The preferences were found", typeof(StaffPreferencesResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No preferences exist yet for this staff member")]
    public async Task<IActionResult> GetPreferences([FromRoute] Guid staffUserId, CancellationToken cancellationToken)
    {
        var preferences = await queryService.Handle(new GetStaffPreferencesByUserIdQuery(staffUserId), cancellationToken);
        if (preferences is null)
            return ProfilesActionResultAssembler.ToProblemDetails(
                this, new ProfilesError(ProfilesErrors.StaffPreferencesNotFound), localizer, problemDetailsFactory);

        return Ok(StaffResourceFromEntityAssembler.ToResourceFromEntity(preferences));
    }

    [HttpPut("preferences")]
    [SwaggerOperation(
        Summary = "Set a staff member's notification preferences",
        Description = "Critical safety alerts can never be turned off and are always returned as enabled.",
        OperationId = "UpdateStaffPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The preferences were saved", typeof(StaffPreferencesResource))]
    public async Task<IActionResult> UpdatePreferences(
        [FromRoute] Guid staffUserId, [FromBody] UpdateStaffNotificationPreferencesResource resource, CancellationToken cancellationToken)
    {
        var preferences = await commandService.Handle(
            new UpdateStaffNotificationPreferencesCommand(staffUserId, resource.PendingRedemptionsEnabled, resource.NewBookingsEnabled),
            cancellationToken);
        return Ok(StaffResourceFromEntityAssembler.ToResourceFromEntity(preferences));
    }
}
