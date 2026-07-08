namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Resources;

public record SubscriptionResource(
    Guid Id,
    string Plan,
    string Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate);
