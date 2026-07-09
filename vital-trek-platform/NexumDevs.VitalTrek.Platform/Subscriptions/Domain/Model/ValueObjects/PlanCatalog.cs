namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

/**
 * <summary>
 *     Static price list for the three supported paid tiers. Academic/demo pricing —
 *     TODO: move to a configurable table (or Stripe Price IDs) if real billing is needed later.
 * </summary>
 */
public static class PlanCatalog
{
    public static readonly IReadOnlyDictionary<SubscriptionPlan, (long AmountCents, string Currency, string DisplayName)> Plans =
        new Dictionary<SubscriptionPlan, (long AmountCents, string Currency, string DisplayName)>
        {
            [SubscriptionPlan.TrekkerAdventurer] = (1900, "pen", "Vital Trek Adventurer"),
            [SubscriptionPlan.AgencyBase] = (29900, "pen", "Vital Trek Agency Base"),
            [SubscriptionPlan.AgencyPro] = (59900, "pen", "Vital Trek Agency Pro")
        };

    public static (long AmountCents, string Currency, string DisplayName) Get(SubscriptionPlan plan) => Plans[plan];
}
