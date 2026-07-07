namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

/// <summary>
/// Represents the request payload for partially updating a support ticket.
/// At least one of <paramref name="Status"/> or <paramref name="Priority"/> must be provided.
/// </summary>
/// <param name="Status">The new status, or <c>null</c> to leave it unchanged.</param>
/// <param name="Priority">The new priority, or <c>null</c> to leave it unchanged.</param>
public record UpdateTicketResource(string? Status, string? Priority);
