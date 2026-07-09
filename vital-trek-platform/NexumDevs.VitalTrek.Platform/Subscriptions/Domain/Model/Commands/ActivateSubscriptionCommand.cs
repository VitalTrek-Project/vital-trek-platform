namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;

/**
 * <summary>
 *     Activates the subscription tied to a checkout session. Issued when the payment gateway
 *     confirms the payment succeeded (mock-checkout confirm endpoint, or a real gateway's
 *     webhook). EndDate is derived from the subscription's own Plan, not passed in here.
 * </summary>
 */
public record ActivateSubscriptionCommand(
    string StripeCheckoutSessionId,
    string? StripeSubscriptionId,
    DateTimeOffset StartDate);
