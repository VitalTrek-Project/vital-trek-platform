using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class RedemptionResourceAssembler
{
    public static RedemptionResource ToResourceFromEntity(Redemption entity) => new(
        entity.Id, entity.TouristId, entity.RewardId, entity.PointsSpent, entity.Code,
        entity.Status.ToString(), entity.CreatedAt, entity.UsedAt, entity.ExpiresAt);
}
