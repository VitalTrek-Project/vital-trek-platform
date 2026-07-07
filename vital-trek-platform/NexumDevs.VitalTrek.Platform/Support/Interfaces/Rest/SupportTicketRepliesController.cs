using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
[Route("api/v1/[controller]")]
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
    public async Task<IActionResult> AddTicketReply([FromBody] AddTicketReplyResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new AddTicketReplyCommand(
                resource.TicketId,
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
    [SwaggerResponse(StatusCodes.Status404NotFound, "The ticket was not found")]
    public async Task<IActionResult> GetTicketReplies([FromQuery, Required] Guid ticketId, CancellationToken cancellationToken)
    {
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

    private IActionResult ToProblemDetails(SupportError error)
    {
        var statusCode = error.ErrorCode == SupportErrors.TicketNotFound
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status400BadRequest;

        return problemDetailsFactory.CreateProblemDetails(this, statusCode, (Enum?)null, localizer[error.ErrorCode]);
    }
}
