namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;

/// <summary>Request body for starting a checkout. Plan is one of the <see cref="Domain.Model.ValueObjects.SubscriptionPlan" /> names (e.g. "TrekkerAdventurer", "AgencyBase", "AgencyPro").</summary>
public record CreateCheckoutSessionResource(string Plan);
