using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

/// <summary>
/// Represents the request payload for posting a new reply on a ticket's conversation thread.
/// </summary>
/// <param name="TicketId">The identifier of the ticket being replied to.</param>
/// <param name="AuthorName">The display name of the reply author.</param>
/// <param name="AuthorMode">The role of the reply author (Tourist, Guide, or Support).</param>
/// <param name="Message">The reply message body.</param>
public record AddTicketReplyResource(
    [Required] Guid TicketId,
    [Required] string AuthorName,
    [Required] string AuthorMode,
    [Required] string Message);
