using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

/// <summary>
/// Represents the request payload for opening a new support ticket.
/// </summary>
/// <param name="UserId">The identifier of the tourist or guide opening the ticket.</param>
/// <param name="UserMode">The role of the user opening the ticket (Tourist, Guide, or Support).</param>
/// <param name="FullName">The full name of the requester.</param>
/// <param name="Email">The contact email of the requester.</param>
/// <param name="Subject">The ticket subject.</param>
/// <param name="Category">The ticket category.</param>
/// <param name="Description">The detailed description of the issue.</param>
/// <param name="Priority">The initial priority of the ticket.</param>
public record CreateTicketResource(
    [Required] Guid UserId,
    [Required] string UserMode,
    [Required] string FullName,
    [Required, EmailAddress] string Email,
    [Required] string Subject,
    [Required] string Category,
    [Required] string Description,
    [Required] string Priority);
