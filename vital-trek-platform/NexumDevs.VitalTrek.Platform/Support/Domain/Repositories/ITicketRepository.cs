using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;

namespace NexumDevs.VitalTrek.Platform.Support.Domain.Repositories;

/// <summary>
/// Defines the repository contract for managing <see cref="Ticket"/> aggregates.
/// </summary>
public interface ITicketRepository : IBaseRepository<Ticket>
{
    /// <summary>
    /// Retrieves a ticket, including its replies, by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the ticket.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching <see cref="Ticket"/> if found; otherwise, <c>null</c>.</returns>
    Task<Ticket?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves every ticket opened by a specific tourist or guide, including their replies.
    /// </summary>
    /// <param name="userId">The identifier of the tourist or guide.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The tickets opened by the specified user.</returns>
    Task<IEnumerable<Ticket>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
