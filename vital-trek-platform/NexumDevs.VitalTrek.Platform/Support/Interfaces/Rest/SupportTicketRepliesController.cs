using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Support.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Support.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Support.Domain;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Support.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest;

[ApiController]
[Route("api/v1/tickets/{ticketId:guid}/replies")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Support Ticket Reply endpoints")]
public class SupportTicketRepliesController(
    ISupportCommandService commandService,
    ISupportQueryService queryService,
    IStringLocalizer<SupportMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Post a reply on a ticket",
        Description = "Posts a new reply on a support ticket's conversation thread",
        OperationId = "AddTicketReply")]
    [SwaggerResponse(StatusCodes.Status201Created, "The reply was created", typeof(TicketReplyResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The reply was not created")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The ticket was not found")]
    public async Task<IActionResult> AddTicketReply(
        [FromRoute] Guid ticketId, [FromBody] AddTicketReplyResource resource, CancellationToken cancellationToken)
    {
        var forbidden = await CheckOwnershipAsync(ticketId, cancellationToken);
        if (forbidden is not null) return forbidden;

        try
        {
            var command = new AddTicketReplyCommand(
                ticketId,
                resource.AuthorName,
                resource.AuthorMode,
                resource.Message);

            var reply = await commandService.Handle(command, cancellationToken);
            var result = TicketReplyResourceFromEntityAssembler.ToResourceFromEntity(reply);
            return CreatedAtAction(nameof(GetTicketReplies), new { ticketId = result.TicketId }, result);
        }
        catch (SupportError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get replies for a ticket",
        Description = "Gets the conversation thread of replies for a specific ticket",
        OperationId = "GetTicketReplies")]
    [SwaggerResponse(StatusCodes.Status200OK, "The replies were found", typeof(IEnumerable<TicketReplyResource>))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "A tourist tried to read another user's ticket replies")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The ticket was not found")]
    public async Task<IActionResult> GetTicketReplies([FromRoute, Required] Guid ticketId, CancellationToken cancellationToken)
    {
        var forbidden = await CheckOwnershipAsync(ticketId, cancellationToken);
        if (forbidden is not null) return forbidden;

        try
        {
            var replies = await queryService.Handle(new GetTicketRepliesByTicketIdQuery(ticketId), cancellationToken);
            var resources = replies.Select(TicketReplyResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (SupportError error)
        {
            return ToProblemDetails(error);
        }
    }

    /// <summary>
    /// A Tourist caller may only act on their own ticket's replies (mirrors the ownership
    /// check in <see cref="SupportTicketsController.GetTickets" />). Returns a ready-made
    /// <see cref="IActionResult" /> (404/403) when the caller must be blocked, or <c>null</c>
    /// when they may proceed. Uses the untracked <see cref="GetTicketOwnerUserIdQuery" /> —
    /// not <see cref="GetTicketByIdQuery" /> — so this pre-check doesn't also track the ticket;
    /// AddTicketReply's own command handler does a *tracking* fetch of the same ticket right
    /// after, and a second tracking fetch here would confuse EF's change detection for the
    /// new reply added afterward (it started emitting UPDATE instead of INSERT for it).
    /// </summary>
    private async Task<IActionResult?> CheckOwnershipAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var ownerUserId = await queryService.Handle(new GetTicketOwnerUserIdQuery(ticketId), cancellationToken);
        if (ownerUserId is null)
            return ToProblemDetails(new SupportError(SupportErrors.TicketNotFound));

        var isTourist = User.FindFirstValue(ClaimTypes.Role) == nameof(UserRole.Tourist);
        var isOwner = string.Equals(ownerUserId.ToString(), User.FindFirstValue(ClaimTypes.NameIdentifier),
            StringComparison.OrdinalIgnoreCase);

        return isTourist && !isOwner ? Forbid() : null;
    }

    private IActionResult ToProblemDetails(SupportError error)
    {
        var statusCode = error.ErrorCode == SupportErrors.TicketNotFound
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status400BadRequest;

        return problemDetailsFactory.CreateProblemDetails(this, statusCode, (Enum?)null, localizer[error.ErrorCode]);
    }
}
