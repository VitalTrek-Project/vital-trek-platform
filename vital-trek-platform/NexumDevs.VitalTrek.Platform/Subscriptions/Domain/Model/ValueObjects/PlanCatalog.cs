namespace NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;

/**
 * <summary>
 *     Static price list for the two supported plans. Academic/demo pricing —
 *     TODO: move to a configurable table (or Stripe Price IDs) if real billing is needed later.
 * </summary>
 */
public static class PlanCatalog
{
    public static readonly IReadOnlyDictionary<SubscriptionPlan, (long AmountCents, string Currency, string DisplayName)> Plans =
        new Dictionary<SubscriptionPlan, (long AmountCents, string Currency, string DisplayName)>
        {
            [SubscriptionPlan.Monthly] = (999, "usd", "Vital Trek Premium — Monthly"),
            [SubscriptionPlan.Annual] = (9999, "usd", "Vital Trek Premium — Annual")
        };

    public static (long AmountCents, string Currency, string DisplayName) Get(SubscriptionPlan plan) => Plans[plan];
}
