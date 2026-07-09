using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record BadgeDefinitionResource(Guid Id, Guid? AgencyId, string Code, string Name, string Description, string RuleType, int RuleThreshold);

public record CreateBadgeDefinitionResource(
    Guid? AgencyId,
    [Required] string Code,
    [Required] string Name,
    string Description,
    [Required] string RuleType,
    [Range(1, int.MaxValue)] int RuleThreshold);
