namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve a support ticket, including its replies, by its identifier.
/// </summary>
/// <param name="TicketId">The identifier of the ticket to retrieve.</param>
public record GetTicketByIdQuery(Guid TicketId);
