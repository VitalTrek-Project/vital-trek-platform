using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Support.Application.QueryServices;

/// <summary>
/// Defines the contract for handling support-related queries, including retrieving
/// tickets and their conversation threads.
/// </summary>
public interface ISupportQueryService
{
    /// <summary>
    /// Retrieves a ticket, including its replies, by its identifier.
    /// </summary>
    /// <param name="query">The query containing the ticket identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching <see cref="Ticket"/> if found; otherwise, <c>null</c>.</returns>
    Task<Ticket?> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves every support ticket in the system.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>All existing tickets.</returns>
    Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves every ticket opened by a specific tourist or guide.
    /// </summary>
    /// <param name="query">The query containing the user identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The tickets opened by the specified user.</returns>
    Task<IEnumerable<Ticket>> Handle(GetTicketsByUserIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the conversation thread of replies for a specific ticket.
    /// </summary>
    /// <param name="query">The query containing the ticket identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The replies posted on the specified ticket.</returns>
    /// <exception cref="Domain.SupportError">Thrown when the ticket cannot be found.</exception>
    Task<IEnumerable<TicketReply>> Handle(GetTicketRepliesByTicketIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Reads only a ticket's owning user id, for ownership checks — see
    /// <see cref="GetTicketOwnerUserIdQuery" />.
    /// </summary>
    /// <returns>The owning user id, or <c>null</c> if no ticket has this id.</returns>
    Task<Guid?> Handle(GetTicketOwnerUserIdQuery query, CancellationToken cancellationToken);
}
