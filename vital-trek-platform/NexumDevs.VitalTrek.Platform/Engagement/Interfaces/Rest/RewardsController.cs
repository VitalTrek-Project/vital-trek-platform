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

/// <summary>An agency's catalog of redeemable rewards.</summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty reward endpoints")]
public class RewardsController(
    IRewardCommandService commandService,
    IRewardQueryService queryService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("agencies/{agencyId:guid}/rewards")]
    [SwaggerOperation(Summary = "Get an agency's reward catalog", OperationId = "GetRewards")]
    [SwaggerResponse(StatusCodes.Status200OK, "The rewards were found", typeof(IEnumerable<RewardResource>))]
    public async Task<IActionResult> GetRewards([FromRoute] Guid agencyId, [FromQuery] bool? activeOnly, CancellationToken cancellationToken)
    {
        var rewards = await queryService.Handle(new GetRewardsQuery(agencyId, activeOnly), cancellationToken);
        return Ok(rewards.Select(RewardResourceAssembler.ToResourceFromEntity));
    }

    [HttpPost("agencies/{agencyId:guid}/rewards")]
    [SwaggerOperation(Summary = "Create a reward", OperationId = "CreateReward")]
    [SwaggerResponse(StatusCodes.Status201Created, "The reward was created", typeof(RewardResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The reward is invalid")]
    public async Task<IActionResult> CreateReward([FromRoute] Guid agencyId, [FromBody] CreateRewardResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateRewardCommand(agencyId, resource.Name, resource.Description, resource.PointsCost, resource.Stock);
            var reward = await commandService.Handle(command, cancellationToken);
            var result = RewardResourceAssembler.ToResourceFromEntity(reward);
            return CreatedAtAction(nameof(GetRewards), new { agencyId }, result);
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpPut("agencies/{agencyId:guid}/rewards/{rewardId:guid}")]
    [SwaggerOperation(Summary = "Update a reward", OperationId = "UpdateReward")]
    [SwaggerResponse(StatusCodes.Status200OK, "The reward was updated", typeof(RewardResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The reward was not found")]
    public async Task<IActionResult> UpdateReward([FromRoute] Guid agencyId, [FromRoute] Guid rewardId, [FromBody] UpdateRewardResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateRewardCommand(agencyId, rewardId, resource.Name, resource.Description, resource.PointsCost, resource.Stock, resource.IsActive);
            var reward = await commandService.Handle(command, cancellationToken);
            return Ok(RewardResourceAssembler.ToResourceFromEntity(reward));
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpDelete("agencies/{agencyId:guid}/rewards/{rewardId:guid}")]
    [SwaggerOperation(
        Summary = "Deactivate a reward",
        Description = "Deactivates rather than hard-deletes — past redemptions keep a reference to it.",
        OperationId = "DeactivateReward")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The reward was deactivated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The reward was not found")]
    public async Task<IActionResult> DeactivateReward([FromRoute] Guid agencyId, [FromRoute] Guid rewardId, CancellationToken cancellationToken)
    {
        try
        {
            await commandService.Handle(new DeactivateRewardCommand(agencyId, rewardId), cancellationToken);
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
