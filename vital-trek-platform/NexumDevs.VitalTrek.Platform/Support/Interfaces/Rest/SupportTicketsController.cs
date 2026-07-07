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
[SwaggerTag("Available Support Ticket endpoints")]
public class SupportTicketsController(
    ISupportCommandService commandService,
    ISupportQueryService queryService,
    IStringLocalizer<SupportMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Open a support ticket",
        Description = "Opens a new support ticket on behalf of a tourist or guide",
        OperationId = "CreateTicket")]
    [SwaggerResponse(StatusCodes.Status201Created, "The ticket was created", typeof(TicketResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The ticket was not created")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateTicketCommand(
                resource.UserId,
                resource.UserMode,
                resource.FullName,
                resource.Email,
                resource.Subject,
                resource.Category,
                resource.Description,
                resource.Priority);

            var ticket = await commandService.Handle(command, cancellationToken);
            var result = TicketResourceFromEntityAssembler.ToResourceFromEntity(ticket);
            return CreatedAtAction(nameof(GetTicketById), new { ticketId = result.Id }, result);
        }
        catch (SupportError error)
        {
            return ToProblemDetails(error);
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get support tickets",
        Description = "Gets every support ticket, optionally filtered by the user who opened it",
        OperationId = "GetTickets")]
    [SwaggerResponse(StatusCodes.Status200OK, "The tickets were found", typeof(IEnumerable<TicketResource>))]
    public async Task<IActionResult> GetTickets([FromQuery] Guid? userId, CancellationToken cancellationToken)
    {
        var tickets = userId.HasValue
            ? await queryService.Handle(new GetTicketsByUserIdQuery(userId.Value), cancellationToken)
            : await queryService.Handle(new GetAllTicketsQuery(), cancellationToken);

        var resources = tickets.Select(TicketResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{ticketId:guid}")]
    [SwaggerOperation(
        Summary = "Get a ticket by its id",
        Description = "Get a support ticket by its id",
        OperationId = "GetTicketById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The ticket was found", typeof(TicketResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The ticket was not found")]
    public async Task<IActionResult> GetTicketById([FromRoute] Guid ticketId, CancellationToken cancellationToken)
    {
        var ticket = await queryService.Handle(new GetTicketByIdQuery(ticketId), cancellationToken);
        if (ticket is null)
            return ToProblemDetails(new SupportError(SupportErrors.TicketNotFound));

        return Ok(TicketResourceFromEntityAssembler.ToResourceFromEntity(ticket));
    }

    [HttpPatch("{ticketId:guid}")]
    [SwaggerOperation(
        Summary = "Partially update a ticket",
        Description = "Updates a support ticket's status and/or priority",
        OperationId = "UpdateTicket")]
    [SwaggerResponse(StatusCodes.Status200OK, "The ticket was updated", typeof(TicketResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The status or priority value is not recognized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The ticket was not found")]
    public async Task<IActionResult> UpdateTicket(
        [FromRoute] Guid ticketId,
        [FromBody] UpdateTicketResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateTicketCommand(ticketId, resource.Status, resource.Priority);
            var ticket = await commandService.Handle(command, cancellationToken);
            return Ok(TicketResourceFromEntityAssembler.ToResourceFromEntity(ticket));
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
