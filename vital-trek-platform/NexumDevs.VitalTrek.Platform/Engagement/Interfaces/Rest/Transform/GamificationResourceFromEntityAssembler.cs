using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class GamificationResourceFromEntityAssembler
{
    public static GamificationResource ToResourceFromEntity(GamificationProfile entity)
    {
        return new GamificationResource(
            entity.TouristId,
            entity.TotalPoints,
            entity.Rank,
            entity.UnlockedBadges);
    }
}
