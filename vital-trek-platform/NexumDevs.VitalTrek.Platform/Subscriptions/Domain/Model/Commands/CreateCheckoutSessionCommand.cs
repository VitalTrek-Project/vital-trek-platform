using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;

/**
 * <summary>
 *     Starts a Stripe Checkout session for the given user and plan.
 * </summary>
 */
public record CreateCheckoutSessionCommand(Guid UserId, SubscriptionPlan Plan);
