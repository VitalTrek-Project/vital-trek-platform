namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Commands;

/// <summary>
/// Command used to post a new reply on a support ticket's conversation thread.
/// </summary>
/// <param name="TicketId">The identifier of the ticket being replied to.</param>
/// <param name="AuthorName">The display name of the reply author.</param>
/// <param name="AuthorMode">The role of the reply author (Tourist, Guide, or Support).</param>
/// <param name="Message">The reply message body.</param>
public record AddTicketReplyCommand(Guid TicketId, string AuthorName, string AuthorMode, string Message);
