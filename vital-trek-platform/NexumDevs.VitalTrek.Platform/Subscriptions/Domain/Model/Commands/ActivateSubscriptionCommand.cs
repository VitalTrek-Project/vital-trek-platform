namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;

/**
 * <summary>
 *     Activates the subscription tied to a Stripe Checkout session. Issued from the
 *     webhook handler when Stripe reports <c>checkout.session.completed</c>.
 * </summary>
 */
public record ActivateSubscriptionCommand(
    string StripeCheckoutSessionId,
    string? StripeSubscriptionId,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate);
