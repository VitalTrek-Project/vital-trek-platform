namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

/// <summary>
/// Represents the support ticket resource returned by the support API.
/// </summary>
public record TicketResource(
    Guid Id,
    Guid UserId,
    string UserMode,
    string FullName,
    string Email,
    string Subject,
    string Category,
    string Description,
    string Priority,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
