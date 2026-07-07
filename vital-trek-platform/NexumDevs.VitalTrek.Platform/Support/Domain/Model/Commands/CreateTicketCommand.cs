namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Commands;

/// <summary>
/// Command used to open a new support ticket.
/// </summary>
/// <param name="UserId">The identifier of the tourist or guide opening the ticket.</param>
/// <param name="UserMode">The role of the user opening the ticket (Tourist, Guide, or Support).</param>
/// <param name="FullName">The full name of the requester.</param>
/// <param name="Email">The contact email of the requester.</param>
/// <param name="Subject">The ticket subject.</param>
/// <param name="Category">The ticket category.</param>
/// <param name="Description">The detailed description of the issue.</param>
/// <param name="Priority">The initial priority of the ticket.</param>
public record CreateTicketCommand(
    Guid UserId,
    string UserMode,
    string FullName,
    string Email,
    string Subject,
    string Category,
    string Description,
    string Priority);
