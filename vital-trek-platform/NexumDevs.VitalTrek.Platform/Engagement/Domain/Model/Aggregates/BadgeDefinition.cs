using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root describing an awardable badge. This is the extensibility point
/// requested for the badge system: adding a new badge is inserting a new row here
/// (via seed data or the catalog endpoint), never a migration.
/// A <c>null</c> <see cref="AgencyId"/> marks a platform-wide badge available to
/// every agency's program; a non-null value scopes it to one agency's custom catalog.
/// </summary>
public class BadgeDefinition
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected BadgeDefinition() { }

    public BadgeDefinition(Guid? agencyId, string code, string name, string description, BadgeRuleType ruleType, int ruleThreshold)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            throw new EngagementError(EngagementErrors.InvalidTierConfiguration, "Badge code and name are required.");

        if (ruleType != BadgeRuleType.Custom && ruleThreshold <= 0)
            throw new EngagementError(EngagementErrors.InvalidTierConfiguration, "Badge threshold must be greater than zero.");

        Id = Guid.NewGuid();
        AgencyId = agencyId;
        Code = code;
        Name = name;
        Description = description ?? string.Empty;
        RuleType = ruleType;
        RuleThreshold = ruleThreshold;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid? AgencyId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public BadgeRuleType RuleType { get; private set; }
    public int RuleThreshold { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
