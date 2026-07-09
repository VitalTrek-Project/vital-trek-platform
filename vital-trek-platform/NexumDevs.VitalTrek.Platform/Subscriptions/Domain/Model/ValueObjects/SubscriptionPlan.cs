namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

/**
 * <summary>
 *     The paid tier a subscription is on. All tiers bill monthly today — there is no
 *     annual option. Explorer (the free trekker tier) is not represented here since it
 *     never creates a <see cref="Aggregates.Subscription" /> row.
 * </summary>
 */
public enum SubscriptionPlan
{
    TrekkerAdventurer,
    AgencyBase,
    AgencyPro
}
