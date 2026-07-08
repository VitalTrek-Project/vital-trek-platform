namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;

/**
 * <summary>
 *     Cancels the authenticated user's active subscription.
 * </summary>
 */
public record CancelSubscriptionCommand(Guid UserId);
