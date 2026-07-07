using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Support.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Support.Domain;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Support.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Support.Application.Internal.CommandServices;

/// <summary>
/// Command service responsible for handling support use cases, including opening
/// tickets, posting replies, and updating a ticket's status or priority.
/// </summary>
public class SupportCommandService : ISupportCommandService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportCommandService"/> class.
    /// </summary>
    /// <param name="ticketRepository">Repository used to manage support tickets.</param>
    /// <param name="unitOfWork">Unit of Work used to persist changes.</param>
    public SupportCommandService(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Ticket> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = new Ticket(
            command.UserId,
            command.UserMode,
            command.FullName,
            command.Email,
            command.Subject,
            command.Category,
            command.Description,
            command.Priority);

        await _ticketRepository.AddAsync(ticket, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ticket;
    }

    /// <inheritdoc />
    public async Task<TicketReply> Handle(AddTicketReplyCommand command, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.FindByIdAsync(command.TicketId, cancellationToken);
        if (ticket is null)
            throw new SupportError(SupportErrors.TicketNotFound);

        var reply = ticket.AddReply(command.AuthorName, command.AuthorMode, command.Message);

        _ticketRepository.Update(ticket);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return reply;
    }

    /// <inheritdoc />
    public async Task<Ticket> Handle(UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        if (command.Status is null && command.Priority is null)
            throw new SupportError(SupportErrors.NoUpdateFieldsProvided);

        var ticket = await _ticketRepository.FindByIdAsync(command.TicketId, cancellationToken);
        if (ticket is null)
            throw new SupportError(SupportErrors.TicketNotFound);

        if (command.Status is not null)
            ticket.ChangeStatus(command.Status);

        if (command.Priority is not null)
            ticket.ChangePriority(command.Priority);

        _ticketRepository.Update(ticket);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ticket;
    }
}
