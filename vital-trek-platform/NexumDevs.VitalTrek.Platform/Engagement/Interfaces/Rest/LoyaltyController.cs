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

[ApiController]
[Route("api/v1/loyalty")]
[Authorize]
public class LoyaltyController : ControllerBase
{
    private readonly IEngagementCommandService _commandService;
    private readonly IEngagementQueryService _queryService;
    private readonly IStringLocalizer _localizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    
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
                (Enum?)null,
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
                (Enum?)null,
                _localizer[error.ErrorCode]);
        }
        catch (EngagementError error) when (error.ErrorCode == EngagementErrors.InvalidPoints)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                (Enum?)null,
                _localizer[error.ErrorCode]);
        }
    }
}
