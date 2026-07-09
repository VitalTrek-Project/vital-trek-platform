using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Support.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="ITicketRepository"/>.
/// Provides persistence operations for <see cref="Ticket"/> aggregates.
/// </summary>
public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TicketRepository"/> class.
    /// </summary>
    /// <param name="context">
    /// The application database context used to access persistence storage.
    /// </param>
    public TicketRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a ticket, including its replies, by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the ticket.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching <see cref="Ticket"/> if found; otherwise, <c>null</c>.</returns>
    public async Task<Ticket?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<Ticket>()
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves every ticket opened by a specific tourist or guide, including their replies.
    /// </summary>
    /// <param name="userId">The identifier of the tourist or guide.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The tickets opened by the specified user.</returns>
    public async Task<IEnumerable<Ticket>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<Ticket>()
            .Include(t => t.Replies)
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves every ticket, including their replies.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>All existing tickets.</returns>
    public new async Task<IEnumerable<Ticket>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Ticket>()
            .Include(t => t.Replies)
            .ToListAsync(cancellationToken);
    }

    public async Task<Guid?> FindOwnerUserIdAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        return await Context.Set<Ticket>()
            .AsNoTracking()
            .Where(t => t.Id == ticketId)
            .Select(t => (Guid?)t.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
