using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class RewardResourceAssembler
{
    public static RewardResource ToResourceFromEntity(Reward entity) =>
        new(entity.Id, entity.Name, entity.Description, entity.PointsCost, entity.Stock, entity.IsActive);
}
