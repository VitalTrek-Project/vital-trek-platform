using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>
/// A tourist's loyalty standing within one agency: balance, tier progress, ledger
/// history, badges, referral code and reviews. touristId is an explicit path
/// parameter — see the IAM TODOs, there is no session to derive "the current tourist" from.
/// </summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty profile endpoints")]
public class ProfileController(
    IProfileCommandService commandService,
    IProfileQueryService queryService,
    IProgramQueryService programQueryService,
    IBadgeQueryService badgeQueryService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}/profile")]
    [SwaggerOperation(Summary = "Get a tourist's loyalty profile", OperationId = "GetLoyaltyProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was found", typeof(LoyaltyProfileResource))]
    public async Task<IActionResult> GetProfile([FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var profile = await queryService.Handle(new GetLoyaltyProfileQuery(agencyId, touristId), cancellationToken);
        var tiers = await programQueryService.Handle(new GetLoyaltyTiersQuery(agencyId), cancellationToken);
        return Ok(ProfileResourceAssembler.ToResourceFromProfile(profile, tiers));
    }

    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}/points-transactions")]
    [SwaggerOperation(Summary = "Get a tourist's points ledger", OperationId = "GetPointsTransactions")]
    [SwaggerResponse(StatusCodes.Status200OK, "The transactions were found", typeof(IEnumerable<PointsTransactionResource>))]
    public async Task<IActionResult> GetTransactions([FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var transactions = await queryService.Handle(new GetPointsTransactionsQuery(agencyId, touristId), cancellationToken);
        return Ok(transactions.Select(ProfileResourceAssembler.ToResourceFromEntity));
    }

    [HttpPost("agencies/{agencyId:guid}/tourists/{touristId:guid}/points-transactions")]
    [SwaggerOperation(
        Summary = "Record a points-earning event",
        Description = "The backend resolves how many points the event is worth from the agency's program — callers never supply a point amount.",
        OperationId = "RecordPointsEvent")]
    [SwaggerResponse(StatusCodes.Status201Created, "The event was recorded", typeof(PointsTransactionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The event type is not recognized")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "This source has already been recorded")]
    public async Task<IActionResult> RecordEvent([FromRoute] Guid agencyId, [FromRoute] Guid touristId, [FromBody] RecordPointsEventResource resource, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<PointsTransactionType>(resource.Type, ignoreCase: true, out var type))
            return problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, (Enum?)null, localizer[EngagementErrors.InvalidEventType]);

        try
        {
            var command = new RecordPointsEventCommand(agencyId, touristId, type, resource.SourceId);
            var transaction = await commandService.Handle(command, cancellationToken);
            var result = ProfileResourceAssembler.ToResourceFromEntity(transaction);
            return CreatedAtAction(nameof(GetTransactions), new { agencyId, touristId }, result);
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}/badges")]
    [SwaggerOperation(Summary = "Get a tourist's earned badges", OperationId = "GetAwardedBadges")]
    [SwaggerResponse(StatusCodes.Status200OK, "The badges were found", typeof(IEnumerable<AwardedBadgeResource>))]
    public async Task<IActionResult> GetBadges([FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var awarded = await queryService.Handle(new GetAwardedBadgesQuery(agencyId, touristId), cancellationToken);
        var catalog = await badgeQueryService.Handle(new GetBadgeCatalogQuery(agencyId), cancellationToken);
        var catalogById = catalog.ToDictionary(b => b.Id);

        var resources = awarded
            .Where(a => catalogById.ContainsKey(a.BadgeDefinitionId))
            .Select(a => ProfileResourceAssembler.ToResourceFromEntity(a, catalogById[a.BadgeDefinitionId]));

        return Ok(resources);
    }

    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}/referral-code")]
    [SwaggerOperation(Summary = "Get or create a tourist's referral code", OperationId = "GetReferralCode")]
    [SwaggerResponse(StatusCodes.Status200OK, "The referral code was found", typeof(ReferralCodeResource))]
    public async Task<IActionResult> GetReferralCode([FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var code = await queryService.Handle(new GetOrCreateReferralCodeQuery(agencyId, touristId), cancellationToken);
        return Ok(ProfileResourceAssembler.ToResourceFromEntity(code));
    }

    [HttpPost("agencies/{agencyId:guid}/tourists/{touristId:guid}/reviews")]
    [SwaggerOperation(Summary = "Submit a review for a completed expedition", OperationId = "SubmitReview")]
    [SwaggerResponse(StatusCodes.Status201Created, "The review was recorded", typeof(ReviewResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "This expedition was already reviewed by this tourist")]
    public async Task<IActionResult> SubmitReview([FromRoute] Guid agencyId, [FromRoute] Guid touristId, [FromBody] SubmitReviewResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new SubmitReviewCommand(agencyId, touristId, resource.ExpeditionId, resource.Rating, resource.Comment);
            var review = await commandService.Handle(command, cancellationToken);
            var result = ProfileResourceAssembler.ToResourceFromEntity(review);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (EngagementError error)
        {
            return ToProblemDetails(error);
        }
    }

    private IActionResult ToProblemDetails(EngagementError error) =>
        problemDetailsFactory.CreateProblemDetails(this, EngagementActionResultAssembler.ResolveStatusCode(error), (Enum?)null, localizer[error.ErrorCode]);
}
