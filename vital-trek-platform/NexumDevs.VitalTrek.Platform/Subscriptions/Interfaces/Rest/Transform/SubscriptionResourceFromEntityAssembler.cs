using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription subscription)
    {
        return new SubscriptionResource(
            subscription.Id,
            subscription.Plan.ToString(),
            subscription.Status.ToString(),
            subscription.StartDate,
            subscription.EndDate);
    }
}
