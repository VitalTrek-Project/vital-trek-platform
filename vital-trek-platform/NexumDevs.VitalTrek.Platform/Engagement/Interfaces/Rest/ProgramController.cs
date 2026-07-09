using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>
/// Manages one agency's loyalty program configuration and its tiers. No [Authorize]:
/// see the IAM TODOs across the codebase — agencyId is an explicit path parameter,
/// same convention as Dashboard and Support today.
/// </summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty program & tier endpoints")]
public class ProgramController(
    IProgramCommandService commandService,
    IProgramQueryService queryService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("agencies/{agencyId:guid}/program")]
    [SwaggerOperation(Summary = "Get an agency's loyalty program configuration", OperationId = "GetLoyaltyProgram")]
    [SwaggerResponse(StatusCodes.Status200OK, "The program was found", typeof(LoyaltyProgramResource))]
    public async Task<IActionResult> GetProgram([FromRoute] Guid agencyId, CancellationToken cancellationToken)
    {
        var program = await queryService.Handle(new GetLoyaltyProgramQuery(agencyId), cancellationToken);
        return Ok(ProgramResourceAssembler.ToResourceFromEntity(program));
    }

    [HttpPut("agencies/{agencyId:guid}/program")]
    [SwaggerOperation(Summary = "Update an agency's loyalty program configuration", OperationId = "UpdateLoyaltyProgram")]
    [SwaggerResponse(StatusCodes.Status200OK, "The program was updated", typeof(LoyaltyProgramResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The configuration is invalid")]
    public async Task<IActionResult> UpdateProgram([FromRoute] Guid agencyId, [FromBody] UpdateLoyaltyProgramResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateLoyaltyProgramCommand(
                agencyId, resource.PointsPerExpeditionCompleted, resource.PointsPerExpeditionBooked,
                resource.PointsPerReferral, resource.PointsPerReview, resource.ExpirationMonths);
            var program = await commandService.Handle(command, cancellationToken);
            return Ok(ProgramResourceAssembler.ToResourceFromEntity(program));
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpGet("agencies/{agencyId:guid}/tiers")]
    [SwaggerOperation(Summary = "Get an agency's loyalty tiers", OperationId = "GetLoyaltyTiers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The tiers were found", typeof(IEnumerable<LoyaltyTierResource>))]
    public async Task<IActionResult> GetTiers([FromRoute] Guid agencyId, CancellationToken cancellationToken)
    {
        var tiers = await queryService.Handle(new GetLoyaltyTiersQuery(agencyId), cancellationToken);
        return Ok(tiers.Select(ProgramResourceAssembler.ToResourceFromEntity));
    }

    [HttpPost("agencies/{agencyId:guid}/tiers")]
    [SwaggerOperation(Summary = "Create a loyalty tier", OperationId = "CreateLoyaltyTier")]
    [SwaggerResponse(StatusCodes.Status201Created, "The tier was created", typeof(LoyaltyTierResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The tier is invalid")]
    public async Task<IActionResult> CreateTier([FromRoute] Guid agencyId, [FromBody] CreateTierResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateLoyaltyTierCommand(agencyId, resource.Name, resource.MinPoints, resource.Benefits, resource.SortOrder);
            var tier = await commandService.Handle(command, cancellationToken);
            var result = ProgramResourceAssembler.ToResourceFromEntity(tier);
            return CreatedAtAction(nameof(GetTiers), new { agencyId }, result);
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpPut("agencies/{agencyId:guid}/tiers/{tierId:guid}")]
    [SwaggerOperation(Summary = "Update a loyalty tier", OperationId = "UpdateLoyaltyTier")]
    [SwaggerResponse(StatusCodes.Status200OK, "The tier was updated", typeof(LoyaltyTierResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The tier was not found")]
    public async Task<IActionResult> UpdateTier([FromRoute] Guid agencyId, [FromRoute] Guid tierId, [FromBody] UpdateTierResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateLoyaltyTierCommand(agencyId, tierId, resource.Name, resource.MinPoints, resource.Benefits, resource.SortOrder);
            var tier = await commandService.Handle(command, cancellationToken);
            return Ok(ProgramResourceAssembler.ToResourceFromEntity(tier));
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpDelete("agencies/{agencyId:guid}/tiers/{tierId:guid}")]
    [SwaggerOperation(Summary = "Delete a loyalty tier", OperationId = "DeleteLoyaltyTier")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The tier was deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The tier was not found")]
    public async Task<IActionResult> DeleteTier([FromRoute] Guid agencyId, [FromRoute] Guid tierId, CancellationToken cancellationToken)
    {
        try
        {
            await commandService.Handle(new DeleteLoyaltyTierCommand(agencyId, tierId), cancellationToken);
            return NoContent();
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    private IActionResult ToProblemDetails(EngagementError error) =>
        problemDetailsFactory.CreateProblemDetails(this, EngagementActionResultAssembler.ResolveStatusCode(error), (Enum?)null, localizer[error.ErrorCode]);
}
