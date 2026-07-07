using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.Services;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

public class RedemptionCommandService(
    IRedemptionRepository redemptionRepository,
    IRewardRepository rewardRepository,
    IGamificationProfileRepository profileRepository,
    IPointsTransactionRepository transactionRepository,
    ILoyaltyTierRepository tierRepository,
    INotificationCommandService notificationService,
    PointsReconciler reconciler,
    IUnitOfWork unitOfWork) : IRedemptionCommandService
{
    public async Task<Redemption> Handle(RedeemRewardCommand command, CancellationToken cancellationToken)
    {
        var reward = await rewardRepository.FindByIdAndAgencyAsync(command.RewardId, command.AgencyId, cancellationToken)
                     ?? throw new EngagementError(EngagementErrors.RewardNotFound);

        var profile = await profileRepository.FindByTouristAndAgencyAsync(command.TouristId, command.AgencyId, cancellationToken);
        var isNewProfile = profile is null;
        profile ??= new GamificationProfile(command.TouristId, command.AgencyId);
        if (!isNewProfile) await reconciler.ReconcileAsync(profile, cancellationToken);

        // A DB-level unique/check constraint isn't practical for "balance >= cost" across
        // an aggregate computed from a separate ledger table, so this is enforced here,
        // inside the same unit of work as the deduction below (reward reservation +
        // balance check + ledger entry + profile update all commit together or not at all).
        if (profile.TotalPoints < reward.PointsCost)
            throw new EngagementError(EngagementErrors.InsufficientBalance);

        reward.ReserveForRedemption();
        rewardRepository.Update(reward);

        var redemption = new Redemption(command.AgencyId, command.TouristId, command.RewardId, reward.PointsCost);
        await redemptionRepository.AddAsync(redemption, cancellationToken);

        var transaction = new PointsTransaction(
            profile.Id, command.AgencyId, command.TouristId,
            PointsTransactionType.RewardRedeemed,
            -reward.PointsCost,
            sourceId: redemption.Id.ToString(),
            description: $"Redeemed \"{reward.Name}\".",
            expiresAt: null);
        await transactionRepository.AddAsync(transaction, cancellationToken);

        profile.ApplyPointsDelta(-reward.PointsCost);
        var tiers = (await tierRepository.FindByAgencyIdAsync(command.AgencyId, cancellationToken))
            .OrderByDescending(t => t.MinPoints)
            .ToList();
        profile.RecalculateTier(tiers);

        if (isNewProfile)
            await profileRepository.AddAsync(profile, cancellationToken);
        else
            profileRepository.Update(profile);

        await unitOfWork.CompleteAsync(cancellationToken);
        return redemption;
    }

    public async Task<Redemption> Handle(MarkRedemptionUsedCommand command, CancellationToken cancellationToken)
    {
        var redemption = await redemptionRepository.FindByIdAndAgencyAsync(command.RedemptionId, command.AgencyId, cancellationToken)
                          ?? throw new EngagementError(EngagementErrors.RedemptionNotFound);

        redemption.MarkAsUsed();
        redemptionRepository.Update(redemption);
        await unitOfWork.CompleteAsync(cancellationToken);

        await notificationService.Handle(new CreateNotificationCommand(
            redemption.TouristId, redemption.AgencyId, NotificationType.RedemptionReady,
            "Canje confirmado", "Tu canje fue marcado como entregado por la agencia."), cancellationToken);

        return redemption;
    }
}
