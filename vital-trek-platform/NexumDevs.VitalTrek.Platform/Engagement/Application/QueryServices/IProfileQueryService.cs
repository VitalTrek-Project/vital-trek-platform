using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface IProfileQueryService
{
    /// <summary>Reconciles expired points before returning, so the balance/tier shown are always current.</summary>
    Task<GamificationProfile> Handle(GetLoyaltyProfileQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<PointsTransaction>> Handle(GetPointsTransactionsQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyList<AwardedBadge>> Handle(GetAwardedBadgesQuery query, CancellationToken cancellationToken);
    Task<ReferralCode> Handle(GetOrCreateReferralCodeQuery query, CancellationToken cancellationToken);
}
