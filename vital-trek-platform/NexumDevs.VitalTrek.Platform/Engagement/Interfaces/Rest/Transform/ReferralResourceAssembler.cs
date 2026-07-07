using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;

public static class ReferralResourceAssembler
{
    public static ReferralResource ToResourceFromEntity(Referral entity) =>
        new(entity.Id, entity.Status.ToString(), entity.CreatedAt, entity.CompletedAt);
}
