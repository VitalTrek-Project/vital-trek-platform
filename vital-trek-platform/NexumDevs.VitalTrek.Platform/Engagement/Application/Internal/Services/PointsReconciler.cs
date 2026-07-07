using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.Services;

/// <summary>
/// Lazily expires stale points and recalculates the affected profile's tier. Invoked
/// at the start of any profile read or points-changing operation, so the ledger and
/// the cached balance/tier are always consistent from the caller's point of view —
/// no background job is needed.
/// </summary>
public class PointsReconciler(
    IPointsTransactionRepository transactionRepository,
    IGamificationProfileRepository profileRepository,
    ILoyaltyTierRepository tierRepository,
    IUnitOfWork unitOfWork)
{
    public async Task ReconcileAsync(GamificationProfile profile, CancellationToken cancellationToken)
    {
        var expired = await transactionRepository.FindExpiredPendingAsync(profile.Id, DateTimeOffset.UtcNow, cancellationToken);
        if (expired.Count == 0) return;

        foreach (var earning in expired)
        {
            var expiry = new PointsTransaction(
                profile.Id,
                profile.AgencyId,
                profile.TouristId,
                PointsTransactionType.PointsExpired,
                -earning.Points,
                sourceId: earning.Id.ToString(),
                description: $"{earning.Points} points earned on {earning.CreatedAt:d} expired.",
                expiresAt: null);
            await transactionRepository.AddAsync(expiry, cancellationToken);

            // Clamped: without FIFO lot-tracking, a tourist may have already spent below
            // this lot's original amount — the ledger keeps the true expiring amount for
            // audit purposes, but the cached balance is never allowed to go negative.
            var clampedDelta = -Math.Min(earning.Points, profile.TotalPoints);
            if (clampedDelta != 0) profile.ApplyPointsDelta(clampedDelta);
        }

        var tiers = (await tierRepository.FindByAgencyIdAsync(profile.AgencyId, cancellationToken))
            .OrderByDescending(t => t.MinPoints)
            .ToList();
        profile.RecalculateTier(tiers);

        profileRepository.Update(profile);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
