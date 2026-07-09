using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root of the Support Bounded Context.
/// Represents a support ticket opened by a tourist or guide, together with
/// the conversation thread of replies exchanged with the support team.
/// </summary>
public class Ticket
{
    private readonly List<TicketReply> _replies = new();

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Ticket() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ticket"/> class.
    /// Validates the provided data and opens the ticket with an initial status of <see cref="TicketStatuses.Open"/>.
    /// </summary>
    /// <param name="userId">The identifier of the tourist or guide opening the ticket.</param>
    /// <param name="userMode">The role of the user opening the ticket (Tourist, Guide, or Support).</param>
    /// <param name="fullName">The full name of the requester.</param>
    /// <param name="email">The contact email of the requester.</param>
    /// <param name="subject">The ticket subject.</param>
    /// <param name="category">The ticket category.</param>
    /// <param name="description">The detailed description of the issue.</param>
    /// <param name="priority">The initial priority of the ticket.</param>
    /// <exception cref="SupportError">
    /// Thrown when the user mode or priority is not a recognized value, or when required fields are missing.
    /// </exception>
    public Ticket(Guid userId, string userMode, string fullName, string email, string subject, string category,
        string description, string priority)
    {
        if (!TicketUserModes.IsValid(userMode))
            throw new SupportError(SupportErrors.InvalidUserMode);

        if (!TicketPriorities.IsValid(priority))
            throw new SupportError(SupportErrors.InvalidPriority);

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(description))
            throw new SupportError(SupportErrors.InvalidTicketData);

        Id = Guid.NewGuid();
        UserId = userId;
        UserMode = userMode;
        FullName = fullName;
        Email = email;
        Subject = subject;
        Category = category;
        Description = description;
        Priority = priority;
        Status = TicketStatuses.Open;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of this ticket.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the tourist or guide who opened the ticket.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the role of the user who opened the ticket (Tourist, Guide, or Support).
    /// </summary>
    public string UserMode { get; private set; } = null!;

    /// <summary>
    /// Gets the full name of the requester.
    /// </summary>
    public string FullName { get; private set; } = null!;

    /// <summary>
    /// Gets the contact email of the requester.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Gets the ticket subject.
    /// </summary>
    public string Subject { get; private set; } = null!;

    /// <summary>
    /// Gets the ticket category.
    /// </summary>
    public string Category { get; private set; } = null!;

    /// <summary>
    /// Gets the detailed description of the issue.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Gets the current priority of the ticket.
    /// </summary>
    public string Priority { get; private set; } = null!;

    /// <summary>
    /// Gets the current lifecycle status of the ticket.
    /// </summary>
    public string Status { get; private set; } = null!;

    /// <summary>
    /// Gets the date and time when the ticket was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the ticket was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the conversation thread of replies posted on this ticket, in chronological order.
    /// </summary>
    public IReadOnlyCollection<TicketReply> Replies => _replies.AsReadOnly();

    /// <summary>
    /// Posts a new reply to the ticket's conversation thread.
    /// </summary>
    /// <param name="authorName">The display name of the reply author.</param>
    /// <param name="authorMode">The role of the reply author (Tourist, Guide, or Support).</param>
    /// <param name="message">The reply message body.</param>
    /// <returns>The newly created <see cref="TicketReply"/>.</returns>
    /// <exception cref="SupportError">
    /// Thrown when the author mode is not a recognized value, or the message is empty.
    /// </exception>
    public TicketReply AddReply(string authorName, string authorMode, string message)
    {
        if (!TicketUserModes.IsValid(authorMode))
            throw new SupportError(SupportErrors.InvalidAuthorMode);

        if (string.IsNullOrWhiteSpace(message))
            throw new SupportError(SupportErrors.EmptyReplyMessage);

        var reply = new TicketReply(Id, authorName, authorMode, message);
        _replies.Add(reply);
        UpdatedAt = DateTimeOffset.UtcNow;
        return reply;
    }

    /// <summary>
    /// Changes the lifecycle status of the ticket.
    /// </summary>
    /// <param name="status">The new status.</param>
    /// <exception cref="SupportError">Thrown when the status is not a recognized value.</exception>
    public void ChangeStatus(string status)
    {
        if (!TicketStatuses.IsValid(status))
            throw new SupportError(SupportErrors.InvalidStatus);

        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Changes the priority of the ticket.
    /// </summary>
    /// <param name="priority">The new priority.</param>
    /// <exception cref="SupportError">Thrown when the priority is not a recognized value.</exception>
    public void ChangePriority(string priority)
    {
        if (!TicketPriorities.IsValid(priority))
            throw new SupportError(SupportErrors.InvalidPriority);

        Priority = priority;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
