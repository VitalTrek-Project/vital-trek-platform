using System.Net.Mime;
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
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>Redeeming rewards and the agency admin's redemption code lookup/fulfillment.</summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty redemption endpoints")]
public class RedemptionsController(
    IRedemptionCommandService commandService,
    IRedemptionQueryService queryService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}/redemptions")]
    [SwaggerOperation(Summary = "Get a tourist's redemptions", OperationId = "GetTouristRedemptions")]
    [SwaggerResponse(StatusCodes.Status200OK, "The redemptions were found", typeof(IEnumerable<RedemptionResource>))]
    public async Task<IActionResult> GetForTourist([FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var redemptions = await queryService.Handle(new GetRedemptionsForTouristQuery(agencyId, touristId), cancellationToken);
        return Ok(redemptions.Select(RedemptionResourceAssembler.ToResourceFromEntity));
    }

    [HttpPost("agencies/{agencyId:guid}/tourists/{touristId:guid}/redemptions")]
    [SwaggerOperation(Summary = "Redeem a reward", OperationId = "RedeemReward")]
    [SwaggerResponse(StatusCodes.Status201Created, "The redemption was created", typeof(RedemptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The reward was not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Insufficient points, stock, or the reward is inactive")]
    public async Task<IActionResult> Redeem([FromRoute] Guid agencyId, [FromRoute] Guid touristId, [FromBody] RedeemRewardResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var redemption = await commandService.Handle(new RedeemRewardCommand(agencyId, touristId, resource.RewardId), cancellationToken);
            var result = RedemptionResourceAssembler.ToResourceFromEntity(redemption);
            return CreatedAtAction(nameof(GetForTourist), new { agencyId, touristId }, result);
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpGet("agencies/{agencyId:guid}/redemptions")]
    [SwaggerOperation(
        Summary = "Find a redemption by its code",
        Description = "Used by an agency admin to look up a redemption a tourist presents in person.",
        OperationId = "FindRedemptionByCode")]
    [SwaggerResponse(StatusCodes.Status200OK, "The redemption was found", typeof(RedemptionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The 'code' query parameter is required")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No redemption matches this code")]
    public async Task<IActionResult> FindByCode([FromRoute] Guid agencyId, [FromQuery] string? code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
            return problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, (Enum?)null, "The 'code' query parameter is required.");

        var redemption = await queryService.Handle(new FindRedemptionByCodeQuery(agencyId, code), cancellationToken);
        if (redemption is null)
            return problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status404NotFound, (Enum?)null, localizer[EngagementErrors.RedemptionNotFound]);

        return Ok(RedemptionResourceAssembler.ToResourceFromEntity(redemption));
    }

    [HttpPatch("agencies/{agencyId:guid}/redemptions/{redemptionId:guid}")]
    [SwaggerOperation(Summary = "Mark a redemption as used", OperationId = "MarkRedemptionUsed")]
    [SwaggerResponse(StatusCodes.Status200OK, "The redemption was updated", typeof(RedemptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The redemption was not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The redemption was already used or has expired")]
    public async Task<IActionResult> MarkUsed([FromRoute] Guid agencyId, [FromRoute] Guid redemptionId, [FromBody] MarkRedemptionUsedResource resource, CancellationToken cancellationToken)
    {
        if (!string.Equals(resource.Status, "Used", StringComparison.OrdinalIgnoreCase))
            return problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, (Enum?)null, "Only {\"status\": \"Used\"} is supported.");

        try
        {
            var redemption = await commandService.Handle(new MarkRedemptionUsedCommand(agencyId, redemptionId), cancellationToken);
            return Ok(RedemptionResourceAssembler.ToResourceFromEntity(redemption));
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    private IActionResult ToProblemDetails(EngagementError error) =>
        problemDetailsFactory.CreateProblemDetails(this, EngagementActionResultAssembler.ResolveStatusCode(error), (Enum?)null, localizer[error.ErrorCode]);
}
