namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;

/// <summary>The Stripe-hosted checkout page the frontend should redirect the browser to.</summary>
public record CheckoutSessionResource(string CheckoutUrl);
