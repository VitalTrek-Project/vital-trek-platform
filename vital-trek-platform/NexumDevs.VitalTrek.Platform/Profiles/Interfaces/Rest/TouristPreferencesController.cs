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
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest;

/// <summary>
/// A tourist's expedition, notification and privacy preferences — separate aggregate/lifecycle
/// from <see cref="TouristProfilesController"/>. Split into three focused sub-resources matching
/// the tourist-facing UI's three tabs (expedition / notifications / privacy).
/// </summary>
[ApiController]
[Route("api/v1/profiles/tourists/{touristId:guid}/preferences")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available tourist preferences endpoints")]
public class TouristPreferencesController(
    ITouristPreferencesCommandService commandService,
    ITouristPreferencesQueryService queryService,
    IStringLocalizer<ProfilesMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get a tourist's preferences", OperationId = "GetTouristPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The preferences were found", typeof(TouristPreferencesResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No preferences exist yet for this tourist")]
    public async Task<IActionResult> GetPreferences([FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var preferences = await queryService.Handle(new GetTouristPreferencesByUserIdQuery(touristId), cancellationToken);
        if (preferences is null)
            return ProfilesActionResultAssembler.ToProblemDetails(
                this, new ProfilesError(ProfilesErrors.PreferencesNotFound), localizer, problemDetailsFactory);

        return Ok(TouristPreferencesResourceFromEntityAssembler.ToResourceFromEntity(preferences));
    }

    [HttpPut("expedition")]
    [SwaggerOperation(Summary = "Set expedition preferences", OperationId = "UpdateExpeditionPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The preferences were saved", typeof(TouristPreferencesResource))]
    public async Task<IActionResult> UpdateExpeditionPreferences(
        [FromRoute] Guid touristId, [FromBody] UpdateExpeditionPreferencesResource resource, CancellationToken cancellationToken)
    {
        EDifficultyLevel? difficulty = null;
        if (!string.IsNullOrWhiteSpace(resource.PreferredDifficulty))
        {
            if (!Enum.TryParse<EDifficultyLevel>(resource.PreferredDifficulty, true, out var parsed))
                return problemDetailsFactory.CreateProblemDetails(
                    this, StatusCodes.Status400BadRequest, (Enum?)null,
                    $"'{resource.PreferredDifficulty}' is not a valid difficulty.");
            difficulty = parsed;
        }

        var preferences = await commandService.Handle(
            new UpdateExpeditionPreferencesCommand(
                touristId, resource.PreferredActivityTypes.ToList(), difficulty, resource.DietaryRestrictions.ToList()),
            cancellationToken);
        return Ok(TouristPreferencesResourceFromEntityAssembler.ToResourceFromEntity(preferences));
    }

    [HttpPut("notifications")]
    [SwaggerOperation(
        Summary = "Set notification preferences",
        Description = "Safety alerts can never be turned off and are always returned as enabled.",
        OperationId = "UpdateTouristNotificationPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The preferences were saved", typeof(TouristPreferencesResource))]
    public async Task<IActionResult> UpdateNotificationPreferences(
        [FromRoute] Guid touristId, [FromBody] UpdateTouristNotificationPreferencesResource resource, CancellationToken cancellationToken)
    {
        var preferences = await commandService.Handle(
            new UpdateTouristNotificationPreferencesCommand(touristId, resource.LoyaltyUpdatesEnabled, resource.ExpeditionRemindersEnabled),
            cancellationToken);
        return Ok(TouristPreferencesResourceFromEntityAssembler.ToResourceFromEntity(preferences));
    }

    [HttpPut("privacy")]
    [SwaggerOperation(Summary = "Set privacy preferences", OperationId = "UpdatePrivacyPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The preferences were saved", typeof(TouristPreferencesResource))]
    public async Task<IActionResult> UpdatePrivacyPreferences(
        [FromRoute] Guid touristId, [FromBody] UpdatePrivacyPreferencesResource resource, CancellationToken cancellationToken)
    {
        var preferences = await commandService.Handle(
            new UpdatePrivacyPreferencesCommand(touristId, resource.ProfileVisibleToExpeditionMates), cancellationToken);
        return Ok(TouristPreferencesResourceFromEntityAssembler.ToResourceFromEntity(preferences));
    }
}
