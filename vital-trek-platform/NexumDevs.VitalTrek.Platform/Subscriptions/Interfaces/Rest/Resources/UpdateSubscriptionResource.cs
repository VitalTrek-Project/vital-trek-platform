namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;

/// <summary>Body for PATCH /api/v1/subscriptions/me. Only "Canceled" is a supported transition today.</summary>
public record UpdateSubscriptionResource(string Status);

/// <summary>
/// Body for PATCH /api/v1/subscriptions/mock-checkout/{sessionId} — the mock stand-in for a
/// real gateway's webhook event. Outcome must be "paid" or "canceled".
/// </summary>
public record UpdateMockCheckoutResource(string Outcome);
