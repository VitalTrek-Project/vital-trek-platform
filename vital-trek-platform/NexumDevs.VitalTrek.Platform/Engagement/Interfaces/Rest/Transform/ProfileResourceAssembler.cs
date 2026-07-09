using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class ProfileResourceAssembler
{
    public static LoyaltyProfileResource ToResourceFromProfile(GamificationProfile profile, IReadOnlyList<LoyaltyTier> tiers)
    {
        var currentTier = tiers.FirstOrDefault(t => t.Id == profile.CurrentTierId);
        var nextTier = tiers
            .Where(t => t.MinPoints > profile.TotalPoints)
            .OrderBy(t => t.MinPoints)
            .FirstOrDefault();

        return new LoyaltyProfileResource(
            profile.TouristId,
            profile.AgencyId,
            profile.TotalPoints,
            profile.CurrentTierId,
            currentTier?.Name,
            nextTier?.Name,
            nextTier is null ? null : nextTier.MinPoints - profile.TotalPoints);
    }

    public static PointsTransactionResource ToResourceFromEntity(PointsTransaction entity) => new(
        entity.Id, entity.Type.ToString(), entity.Points, entity.SourceId, entity.Description, entity.ExpiresAt, entity.CreatedAt);

    public static AwardedBadgeResource ToResourceFromEntity(AwardedBadge entity, BadgeDefinition definition) => new(
        entity.Id, entity.BadgeDefinitionId, definition.Code, definition.Name, definition.Description, entity.AwardedAt);

    public static ReferralCodeResource ToResourceFromEntity(ReferralCode entity) => new(entity.Code);

    public static ReviewResource ToResourceFromEntity(Review entity) =>
        new(entity.Id, entity.ExpeditionId, entity.Rating, entity.Comment, entity.CreatedAt);
}
