using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>
/// REST controller responsible for the loyalty and gamification endpoints.
/// Provides endpoints for retrieving a gamification profile and awarding points.
/// </summary>
[ApiController]
[Route("api/v1/loyalty")]
[Authorize]
public class LoyaltyController : ControllerBase
{
    private readonly IEngagementCommandService _commandService;
    private readonly IEngagementQueryService _queryService;
    private readonly IStringLocalizer _localizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoyaltyController"/> class.
    /// </summary>
    /// <param name="commandService">Service responsible for handling engagement commands.</param>
    /// <param name="queryService">Service responsible for handling engagement queries.</param>
    /// <param name="localizer">Localizer used to retrieve translated error messages.</param>
    /// <param name="problemDetailsFactory">Factory used to create standardized problem details responses.</param>
    public LoyaltyController(
        IEngagementCommandService commandService,
        IEngagementQueryService queryService,
        IStringLocalizer localizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _commandService = commandService;
        _queryService = queryService;
        _localizer = localizer;
        _problemDetailsFactory = problemDetailsFactory;
    }

    /// <summary>
    /// Retrieves the gamification profile for the specified tourist.
    /// Returns a base empty state when the tourist has not yet participated in any expedition.
    /// </summary>
    /// <param name="id">The unique identifier of the tourist.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// HTTP 200 with <see cref="GamificationResource"/> on success or when the profile is not yet initialized.
    /// HTTP 401 when no valid access token is provided.
    /// </returns>
    /// <response code="200">The gamification profile was successfully retrieved.</response>
    /// <response code="401">No valid access token was provided.</response>
    [HttpGet("profiles/{id:guid}")]
    public async Task<IActionResult> GetGamificationProfile(
        Guid id,
        CancellationToken cancellationToken)
    {
        var profile = await _queryService.Handle(
            new GetGamificationProfileQuery(id),
            cancellationToken);

        var resource = profile is not null
            ? GamificationResourceFromEntityAssembler.ToResourceFromEntity(profile)
            : new GamificationResource(id, 0, "Novice", Array.Empty<string>());

        return Ok(resource);
    }

    /// <summary>
    /// Awards points to the specified tourist's gamification profile for a completed expedition.
    /// Creates the profile automatically if it does not yet exist.
    /// </summary>
    /// <param name="id">The unique identifier of the tourist.</param>
    /// <param name="resource">The payload containing the expedition identifier and points to award.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// HTTP 201 with the updated <see cref="GamificationResource"/> on success.
    /// HTTP 400 when the payload contains invalid data.
    /// HTTP 409 when the specified expedition has already been awarded to this profile.
    /// </returns>
    /// <response code="201">Points were successfully awarded and the profile was updated.</response>
    /// <response code="400">The request contains invalid data (e.g. non-positive points).</response>
    /// <response code="409">The specified expedition has already been awarded to this profile.</response>
    [HttpPost("profiles/{id:guid}/award")]
    public async Task<IActionResult> AwardPoints(
        Guid id,
        [FromBody] AwardPointsResource resource,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                null,
                _localizer[EngagementErrors.InvalidPoints]);
        }

        try
        {
            var command = new AwardPointsCommand(id, resource.ExpeditionId, resource.Points);
            var profile = await _commandService.Handle(command, cancellationToken);
            var result = GamificationResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (EngagementError error) when (error.ErrorCode == EngagementErrors.ExpeditionAlreadyAwarded)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status409Conflict,
                null,
                _localizer[error.ErrorCode]);
        }
        catch (EngagementError error) when (error.ErrorCode == EngagementErrors.InvalidPoints)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                null,
                _localizer[error.ErrorCode]);
        }
    }
}
