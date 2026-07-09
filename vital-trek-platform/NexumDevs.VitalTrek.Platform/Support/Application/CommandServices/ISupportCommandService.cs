using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Support.Application.CommandServices;

/// <summary>
/// Defines the contract for handling support-related commands, including opening
/// tickets, posting replies, and updating a ticket's status or priority.
/// </summary>
public interface ISupportCommandService
{
    /// <summary>
    /// Opens a new support ticket.
    /// </summary>
    /// <param name="command">The command containing the ticket data.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The newly created <see cref="Ticket"/>.</returns>
    Task<Ticket> Handle(CreateTicketCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Posts a new reply on a ticket's conversation thread.
    /// </summary>
    /// <param name="command">The command containing the reply data.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The newly created <see cref="TicketReply"/>.</returns>
    Task<TicketReply> Handle(AddTicketReplyCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Partially updates a ticket's status and/or priority.
    /// </summary>
    /// <param name="command">The command containing the ticket identifier and the fields to update.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The updated <see cref="Ticket"/>.</returns>
    Task<Ticket> Handle(UpdateTicketCommand command, CancellationToken cancellationToken);
}
