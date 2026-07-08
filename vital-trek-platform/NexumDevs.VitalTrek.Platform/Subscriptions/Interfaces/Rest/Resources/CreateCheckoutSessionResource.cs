namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;

/// <summary>Request body for starting a checkout. Plan is "Monthly" or "Annual".</summary>
public record CreateCheckoutSessionResource(string Plan);
