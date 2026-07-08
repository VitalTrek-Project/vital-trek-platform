namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

/**
 * <summary>
 *     The lifecycle status of a subscription.
 * </summary>
 */
public enum SubscriptionStatus
{
    PendingPayment,
    Active,
    PaymentFailed,
    Canceled,
    Expired
}
