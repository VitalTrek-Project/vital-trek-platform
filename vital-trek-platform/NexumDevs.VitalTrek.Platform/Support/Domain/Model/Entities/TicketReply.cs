namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;

/// <summary>
/// Represents a single reply posted on a support ticket's conversation thread,
/// authored either by the tourist/guide who opened the ticket or by a support agent.
/// </summary>
public class TicketReply
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected TicketReply() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketReply"/> class.
    /// </summary>
    /// <param name="ticketId">The identifier of the parent ticket.</param>
    /// <param name="authorName">The display name of the reply author.</param>
    /// <param name="authorMode">The role of the reply author (Tourist, Guide, or Support).</param>
    /// <param name="message">The reply message body.</param>
    public TicketReply(Guid ticketId, string authorName, string authorMode, string message)
    {
        Id = Guid.NewGuid();
        TicketId = ticketId;
        AuthorName = authorName;
        AuthorMode = authorMode;
        Message = message;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of this reply.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the parent ticket this reply belongs to.
    /// </summary>
    public Guid TicketId { get; private set; }

    /// <summary>
    /// Gets the display name of the reply author.
    /// </summary>
    public string AuthorName { get; private set; } = null!;

    /// <summary>
    /// Gets the role of the reply author (Tourist, Guide, or Support).
    /// </summary>
    public string AuthorMode { get; private set; } = null!;

    /// <summary>
    /// Gets the reply message body.
    /// </summary>
    public string Message { get; private set; } = null!;

    /// <summary>
    /// Gets the date and time when the reply was posted.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
}
