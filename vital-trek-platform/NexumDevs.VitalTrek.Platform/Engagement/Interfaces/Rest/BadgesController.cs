using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>
/// The badge catalog. Extensibility point requested for the badge system: a new badge
/// is a new row here (global when agencyId is omitted), never a schema change.
/// </summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty badge catalog endpoints")]
public class BadgesController(
    IBadgeCommandService commandService,
    IBadgeQueryService queryService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("badges")]
    [SwaggerOperation(Summary = "Get the badge catalog", OperationId = "GetBadgeCatalog")]
    [SwaggerResponse(StatusCodes.Status200OK, "The catalog was found", typeof(IEnumerable<BadgeDefinitionResource>))]
    public async Task<IActionResult> GetCatalog([FromQuery] Guid? agencyId, CancellationToken cancellationToken)
    {
        var catalog = await queryService.Handle(new GetBadgeCatalogQuery(agencyId), cancellationToken);
        return Ok(catalog.Select(BadgeResourceAssembler.ToResourceFromEntity));
    }

    [HttpPost("badges")]
    [SwaggerOperation(Summary = "Create a badge definition", OperationId = "CreateBadgeDefinition")]
    [SwaggerResponse(StatusCodes.Status201Created, "The badge was created", typeof(BadgeDefinitionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The badge is invalid, or the rule type is not recognized")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "A badge with this code already exists")]
    public async Task<IActionResult> CreateBadge([FromBody] CreateBadgeDefinitionResource resource, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<BadgeRuleType>(resource.RuleType, ignoreCase: true, out var ruleType))
            return problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, (Enum?)null, "'ruleType' is not recognized.");

        try
        {
            var command = new CreateBadgeDefinitionCommand(resource.AgencyId, resource.Code, resource.Name, resource.Description, ruleType, resource.RuleThreshold);
            var badge = await commandService.Handle(command, cancellationToken);
            var result = BadgeResourceAssembler.ToResourceFromEntity(badge);
            return CreatedAtAction(nameof(GetCatalog), new { agencyId = badge.AgencyId }, result);
        }
        catch (EngagementError error)
        {
            return problemDetailsFactory.CreateProblemDetails(this, EngagementActionResultAssembler.ResolveStatusCode(error), (Enum?)null, localizer[error.ErrorCode]);
        }
    }
}
