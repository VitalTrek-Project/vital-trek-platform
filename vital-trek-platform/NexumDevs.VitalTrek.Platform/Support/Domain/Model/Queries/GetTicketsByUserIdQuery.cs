namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve every support ticket opened by a specific tourist or guide.
/// </summary>
/// <param name="UserId">The identifier of the tourist or guide.</param>
public record GetTicketsByUserIdQuery(Guid UserId);
