namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

/// <summary>
/// Determines how a badge's progress is evaluated against the tourist's ledger.
/// <see cref="Custom"/> badges are never auto-awarded by the standard evaluator —
/// they exist so the catalog can hold badge definitions meant for manual/future
/// awarding logic without requiring a schema change.
/// </summary>
public enum BadgeRuleType
{
    ExpeditionCount,
    ReviewCount,
    ReferralCount,
    Custom
}
