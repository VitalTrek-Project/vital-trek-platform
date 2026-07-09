using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>
/// Referral redemption. Completion (and the referrer's bonus) only happens when the
/// referred tourist finishes their first expedition — see <c>ProfileController</c>'s
/// points-transactions endpoint, not here.
/// </summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty referral endpoints")]
public class ReferralsController(
    IReferralCommandService commandService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("agencies/{agencyId:guid}/referrals")]
    [SwaggerOperation(
        Summary = "Redeem a referral code",
        Description = "Creates a pending referral for the referred tourist; awards nothing until their first expedition completes.",
        OperationId = "RedeemReferralCode")]
    [SwaggerResponse(StatusCodes.Status201Created, "The referral was created", typeof(ReferralResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The referral code was not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Self-referral, or this tourist already redeemed a code")]
    public async Task<IActionResult> Redeem([FromRoute] Guid agencyId, [FromBody] RedeemReferralCodeResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new RedeemReferralCodeCommand(agencyId, resource.Code, resource.ReferredTouristId);
            var referral = await commandService.Handle(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, ReferralResourceAssembler.ToResourceFromEntity(referral));
        }
        catch (EngagementError error)
        {
            return problemDetailsFactory.CreateProblemDetails(this, EngagementActionResultAssembler.ResolveStatusCode(error), (Enum?)null, localizer[error.ErrorCode]);
        }
    }
}
