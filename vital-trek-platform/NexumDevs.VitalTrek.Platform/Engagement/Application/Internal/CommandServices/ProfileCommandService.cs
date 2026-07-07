using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.Services;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

/// <summary>
/// Orchestrates the points-earning workflow: recording a ledger entry, updating the
/// profile's cached balance and tier, completing referrals, evaluating badges, and
/// raising notifications. This is intentionally the single place that touches all of
/// those concerns together, since a single points event genuinely cascades through them.
/// </summary>
public class ProfileCommandService(
    IGamificationProfileRepository profileRepository,
    IPointsTransactionRepository transactionRepository,
    ILoyaltyProgramRepository programRepository,
    ILoyaltyTierRepository tierRepository,
    IReviewRepository reviewRepository,
    IReferralRepository referralRepository,
    IBadgeDefinitionRepository badgeDefinitionRepository,
    IAwardedBadgeRepository awardedBadgeRepository,
    INotificationCommandService notificationService,
    PointsReconciler reconciler,
    IUnitOfWork unitOfWork) : IProfileCommandService
{
    public async Task<PointsTransaction> Handle(RecordPointsEventCommand command, CancellationToken cancellationToken)
    {
        return await RecordEventInternalAsync(command.AgencyId, command.TouristId, command.Type, command.SourceId, cancellationToken);
    }

    public async Task<Review> Handle(SubmitReviewCommand command, CancellationToken cancellationToken)
    {
        if (await reviewRepository.ExistsAsync(command.AgencyId, command.TouristId, command.ExpeditionId, cancellationToken))
            throw new EngagementError(EngagementErrors.DuplicateReview);

        var review = new Review(command.AgencyId, command.TouristId, command.ExpeditionId, command.Rating, command.Comment);
        await reviewRepository.AddAsync(review, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await RecordEventInternalAsync(command.AgencyId, command.TouristId, PointsTransactionType.ReviewSubmitted, review.Id.ToString(), cancellationToken);

        return review;
    }

    private async Task<PointsTransaction> RecordEventInternalAsync(
        Guid agencyId, Guid touristId, PointsTransactionType type, string? sourceId, CancellationToken cancellationToken)
    {
        var profile = await GetOrCreateProfileAsync(agencyId, touristId, cancellationToken);
        await reconciler.ReconcileAsync(profile, cancellationToken);

        if (!string.IsNullOrEmpty(sourceId) && await transactionRepository.ExistsAsync(agencyId, touristId, type, sourceId, cancellationToken))
            throw new EngagementError(EngagementErrors.DuplicateTransaction);

        var program = await GetOrCreateProgramAsync(agencyId, cancellationToken);
        var points = program.ResolvePointsFor(type);
        var expiresAt = program.ComputeExpiresAt();
        var description = DescribeEvent(type);

        var transaction = new PointsTransaction(profile.Id, agencyId, touristId, type, points, sourceId, description, expiresAt);
        await transactionRepository.AddAsync(transaction, cancellationToken);

        var previousTierId = profile.CurrentTierId;
        profile.ApplyPointsDelta(points);

        var tiers = (await tierRepository.FindByAgencyIdAsync(agencyId, cancellationToken))
            .OrderByDescending(t => t.MinPoints)
            .ToList();
        profile.RecalculateTier(tiers);

        profileRepository.Update(profile);
        await unitOfWork.CompleteAsync(cancellationToken);

        await notificationService.Handle(new CreateNotificationCommand(
            touristId, agencyId, NotificationType.PointsEarned,
            "¡Ganaste puntos!", $"Ganaste {points} puntos. {description}"), cancellationToken);

        if (profile.CurrentTierId != previousTierId && profile.CurrentTierId is not null)
        {
            var newTier = tiers.First(t => t.Id == profile.CurrentTierId);
            await notificationService.Handle(new CreateNotificationCommand(
                touristId, agencyId, NotificationType.TierUp,
                "¡Subiste de nivel!", $"Ahora eres {newTier.Name}."), cancellationToken);
        }

        if (type == PointsTransactionType.ExpeditionCompleted)
            await TryCompleteReferralAsync(agencyId, touristId, cancellationToken);

        await EvaluateBadgesAsync(agencyId, touristId, cancellationToken);

        return transaction;
    }

    /// <summary>
    /// Completes the referred tourist's pending referral, if any, the moment they finish
    /// their first expedition — never on sign-up — then awards the referrer's bonus.
    /// </summary>
    private async Task TryCompleteReferralAsync(Guid agencyId, Guid referredTouristId, CancellationToken cancellationToken)
    {
        var completedCount = await transactionRepository.CountByTypeAsync(
            agencyId, referredTouristId, PointsTransactionType.ExpeditionCompleted, cancellationToken);
        if (completedCount != 1) return;

        var referral = await referralRepository.FindPendingByReferredTouristAsync(agencyId, referredTouristId, cancellationToken);
        if (referral is null) return;

        referral.Complete();
        referralRepository.Update(referral);
        await unitOfWork.CompleteAsync(cancellationToken);

        await RecordEventInternalAsync(agencyId, referral.ReferrerTouristId, PointsTransactionType.ReferralBonus, referral.Id.ToString(), cancellationToken);
    }

    private async Task EvaluateBadgesAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken)
    {
        var catalog = await badgeDefinitionRepository.FindCatalogAsync(agencyId, cancellationToken);

        foreach (var badge in catalog)
        {
            if (badge.RuleType == BadgeRuleType.Custom) continue;
            if (await awardedBadgeRepository.ExistsAsync(agencyId, touristId, badge.Id, cancellationToken)) continue;

            var progress = badge.RuleType switch
            {
                BadgeRuleType.ExpeditionCount => await transactionRepository.CountByTypeAsync(
                    agencyId, touristId, PointsTransactionType.ExpeditionCompleted, cancellationToken),
                BadgeRuleType.ReviewCount => await reviewRepository.CountByTouristAsync(agencyId, touristId, cancellationToken),
                BadgeRuleType.ReferralCount => await referralRepository.CountCompletedByReferrerAsync(agencyId, touristId, cancellationToken),
                _ => 0
            };

            if (progress < badge.RuleThreshold) continue;

            var awarded = new AwardedBadge(agencyId, touristId, badge.Id);
            await awardedBadgeRepository.AddAsync(awarded, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await notificationService.Handle(new CreateNotificationCommand(
                touristId, agencyId, NotificationType.BadgeEarned,
                "¡Nuevo badge!", $"Desbloqueaste el badge \"{badge.Name}\"."), cancellationToken);
        }
    }

    private async Task<GamificationProfile> GetOrCreateProfileAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByTouristAndAgencyAsync(touristId, agencyId, cancellationToken);
        if (profile is not null) return profile;

        profile = new GamificationProfile(touristId, agencyId);
        await profileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return profile;
    }

    private async Task<LoyaltyProgram> GetOrCreateProgramAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        var program = await programRepository.FindByAgencyIdAsync(agencyId, cancellationToken);
        if (program is not null) return program;

        program = LoyaltyProgram.CreateDefault(agencyId);
        await programRepository.AddAsync(program, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return program;
    }

    private static string DescribeEvent(PointsTransactionType type) => type switch
    {
        PointsTransactionType.ExpeditionCompleted => "Expedición completada.",
        PointsTransactionType.ExpeditionBooked => "Nueva expedición reservada.",
        PointsTransactionType.ReferralBonus => "Bono por referido.",
        PointsTransactionType.ReviewSubmitted => "Reseña enviada.",
        _ => "Ajuste de puntos."
    };
}
