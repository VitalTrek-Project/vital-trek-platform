namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;

/// <summary>
/// Lightweight query used for ownership checks (see <see cref="Repositories.ITicketRepository.FindOwnerUserIdAsync" />) —
/// reads only the owning user id, untracked, so it doesn't interfere with a command handler's
/// own tracking fetch of the same ticket later in the same request.
/// </summary>
/// <param name="TicketId">The identifier of the ticket to look up.</param>
public record GetTicketOwnerUserIdQuery(Guid TicketId);
