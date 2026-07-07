using NexumDevs.VitalTrek.Platform.Support.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Support.Domain;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Support.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Support.Application.Internal.QueryServices;

/// <summary>
/// Query service responsible for handling support-related read operations, including
/// retrieving tickets and their conversation threads.
/// </summary>
public class SupportQueryService : ISupportQueryService
{
    private readonly ITicketRepository _ticketRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportQueryService"/> class.
    /// </summary>
    /// <param name="ticketRepository">Repository used to retrieve support tickets.</param>
    public SupportQueryService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    /// <inheritdoc />
    public async Task<Ticket?> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
    {
        return await _ticketRepository.FindByIdAsync(query.TicketId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery query, CancellationToken cancellationToken)
    {
        return await _ticketRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Ticket>> Handle(GetTicketsByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await _ticketRepository.FindByUserIdAsync(query.UserId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TicketReply>> Handle(GetTicketRepliesByTicketIdQuery query, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.FindByIdAsync(query.TicketId, cancellationToken);
        if (ticket is null)
            throw new SupportError(SupportErrors.TicketNotFound);

        return ticket.Replies;
    }
}
