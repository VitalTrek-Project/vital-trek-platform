namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model;

public enum SubscriptionsError
{
    None,
    SubscriptionNotFound,
    AlreadyActive,
    InvalidPlan,
    StripeNotConfigured,
    StripeError,
    DatabaseError,
    OperationCancelled,
    InternalServerError
}
