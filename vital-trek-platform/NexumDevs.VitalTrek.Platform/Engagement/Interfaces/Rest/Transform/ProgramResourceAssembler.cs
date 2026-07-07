using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class ProgramResourceAssembler
{
    public static LoyaltyProgramResource ToResourceFromEntity(LoyaltyProgram entity) => new(
        entity.Id, entity.AgencyId, entity.PointsPerExpeditionCompleted, entity.PointsPerExpeditionBooked,
        entity.PointsPerReferral, entity.PointsPerReview, entity.ExpirationMonths);

    public static LoyaltyTierResource ToResourceFromEntity(LoyaltyTier entity) =>
        new(entity.Id, entity.Name, entity.MinPoints, entity.Benefits, entity.SortOrder);
}
