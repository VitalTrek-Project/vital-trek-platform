using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record CreateBadgeDefinitionCommand(Guid? AgencyId, string Code, string Name, string Description, BadgeRuleType RuleType, int RuleThreshold);
