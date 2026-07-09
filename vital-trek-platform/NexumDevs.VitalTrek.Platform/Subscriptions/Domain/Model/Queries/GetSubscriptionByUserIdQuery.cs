namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Queries;

/**
 * <summary>
 *     Fetches the most recent subscription for a user.
 * </summary>
 */
public record GetSubscriptionByUserIdQuery(Guid UserId);
