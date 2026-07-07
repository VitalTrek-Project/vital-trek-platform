namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Commands;

/// <summary>
/// Command used to partially update a support ticket's status and/or priority.
/// At least one of <paramref name="Status"/> or <paramref name="Priority"/> must be provided.
/// </summary>
/// <param name="TicketId">The identifier of the ticket to update.</param>
/// <param name="Status">The new status, or <c>null</c> to leave it unchanged.</param>
/// <param name="Priority">The new priority, or <c>null</c> to leave it unchanged.</param>
public record UpdateTicketCommand(Guid TicketId, string? Status, string? Priority);
