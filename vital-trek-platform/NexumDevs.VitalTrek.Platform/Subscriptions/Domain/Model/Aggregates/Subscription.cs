using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;

/**
 * <summary>
 *     The subscription aggregate. One row per checkout attempt: a user who retries after a
 *     failed payment gets a new row rather than mutating history away.
 * </summary>
 */
public class Subscription
{
    protected Subscription()
    {
    }

    public Subscription(Guid userId, SubscriptionPlan plan, string stripeCheckoutSessionId, string? stripeCustomerId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Plan = plan;
        Status = SubscriptionStatus.PendingPayment;
        StripeCheckoutSessionId = stripeCheckoutSessionId;
        StripeCustomerId = stripeCustomerId;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public SubscriptionPlan Plan { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTimeOffset? StartDate { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public string? StripeCheckoutSessionId { get; private set; }
    public string? StripeSubscriptionId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public void Activate(string? stripeSubscriptionId, DateTimeOffset startDate, DateTimeOffset? endDate)
    {
        Status = SubscriptionStatus.Active;
        StripeSubscriptionId = stripeSubscriptionId;
        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkPaymentFailed()
    {
        Status = SubscriptionStatus.PaymentFailed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        Status = SubscriptionStatus.Canceled;
        EndDate = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
