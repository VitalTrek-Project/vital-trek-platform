namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

/// <summary>
/// Represents the ticket reply resource returned by the support API.
/// </summary>
public record TicketReplyResource(
    Guid Id,
    Guid TicketId,
    string AuthorName,
    string AuthorMode,
    string Message,
    DateTimeOffset CreatedAt);
