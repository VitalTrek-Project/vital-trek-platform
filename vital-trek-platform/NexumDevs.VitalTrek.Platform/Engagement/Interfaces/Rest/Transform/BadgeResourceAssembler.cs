using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class BadgeResourceAssembler
{
    public static BadgeDefinitionResource ToResourceFromEntity(BadgeDefinition entity) =>
        new(entity.Id, entity.AgencyId, entity.Code, entity.Name, entity.Description, entity.RuleType.ToString(), entity.RuleThreshold);
}
