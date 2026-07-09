namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve the conversation thread of replies for a specific ticket.
/// </summary>
/// <param name="TicketId">The identifier of the ticket whose replies are requested.</param>
public record GetTicketRepliesByTicketIdQuery(Guid TicketId);
