using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class MetricsQueryService(
    IPointsTransactionRepository transactionRepository,
    IGamificationProfileRepository profileRepository,
    ILoyaltyTierRepository tierRepository) : IMetricsQueryService
{
    private static readonly PointsTransactionType[] IssuedTypes =
    [
        PointsTransactionType.ExpeditionCompleted,
        PointsTransactionType.ExpeditionBooked,
        PointsTransactionType.ReferralBonus,
        PointsTransactionType.ReviewSubmitted
    ];

    public async Task<LoyaltyMetrics> Handle(GetLoyaltyMetricsQuery query, CancellationToken cancellationToken)
    {
        var pointsIssued = await transactionRepository.SumPointsByTypeAsync(query.AgencyId, IssuedTypes, cancellationToken);
        var pointsRedeemedRaw = await transactionRepository.SumPointsByTypeAsync(
            query.AgencyId, [PointsTransactionType.RewardRedeemed], cancellationToken);

        var profiles = await profileRepository.FindByAgencyIdAsync(query.AgencyId, cancellationToken);
        var tiers = await tierRepository.FindByAgencyIdAsync(query.AgencyId, cancellationToken);
        var tierNamesById = tiers.ToDictionary(t => t.Id, t => t.Name);

        var touristsPerTier = profiles
            .GroupBy(p => p.CurrentTierId)
            .Select(g => new TierDistribution(
                g.Key,
                g.Key.HasValue && tierNamesById.TryGetValue(g.Key.Value, out var name) ? name : "Sin nivel",
                g.Count()))
            .OrderBy(t => t.TierName)
            .ToList();

        var topTourists = profiles
            .OrderByDescending(p => p.TotalPoints)
            .Take(10)
            .Select(p => new TopTourist(
                p.TouristId,
                p.TotalPoints,
                p.CurrentTierId.HasValue && tierNamesById.TryGetValue(p.CurrentTierId.Value, out var name) ? name : null))
            .ToList();

        return new LoyaltyMetrics(
            pointsIssued,
            Math.Abs(pointsRedeemedRaw),
            profiles.Count,
            touristsPerTier,
            topTourists);
    }
}
