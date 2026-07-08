namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;

/**
 * <summary>
 *     Marks a pending subscription as payment-failed. Issued from the webhook handler
 *     when Stripe reports <c>invoice.payment_failed</c>.
 * </summary>
 */
public record MarkPaymentFailedCommand(string StripeCheckoutSessionId);
