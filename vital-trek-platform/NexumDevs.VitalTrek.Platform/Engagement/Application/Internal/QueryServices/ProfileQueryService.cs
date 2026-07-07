using NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.Services;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class ProfileQueryService(
    IGamificationProfileRepository profileRepository,
    IPointsTransactionRepository transactionRepository,
    IAwardedBadgeRepository awardedBadgeRepository,
    IReferralCodeRepository referralCodeRepository,
    PointsReconciler reconciler,
    IUnitOfWork unitOfWork) : IProfileQueryService
{
    public async Task<GamificationProfile> Handle(GetLoyaltyProfileQuery query, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByTouristAndAgencyAsync(query.TouristId, query.AgencyId, cancellationToken);
        if (profile is null)
        {
            profile = new GamificationProfile(query.TouristId, query.AgencyId);
            await profileRepository.AddAsync(profile, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return profile;
        }

        await reconciler.ReconcileAsync(profile, cancellationToken);
        return profile;
    }

    public async Task<IReadOnlyList<PointsTransaction>> Handle(GetPointsTransactionsQuery query, CancellationToken cancellationToken)
    {
        var profile = await Handle(new GetLoyaltyProfileQuery(query.AgencyId, query.TouristId), cancellationToken);
        return await transactionRepository.FindByProfileAsync(profile.Id, cancellationToken);
    }

    public async Task<IReadOnlyList<AwardedBadge>> Handle(GetAwardedBadgesQuery query, CancellationToken cancellationToken)
    {
        return await awardedBadgeRepository.FindByTouristAsync(query.AgencyId, query.TouristId, cancellationToken);
    }

    public async Task<ReferralCode> Handle(GetOrCreateReferralCodeQuery query, CancellationToken cancellationToken)
    {
        var code = await referralCodeRepository.FindByTouristAndAgencyAsync(query.TouristId, query.AgencyId, cancellationToken);
        if (code is not null) return code;

        code = new ReferralCode(query.AgencyId, query.TouristId);
        await referralCodeRepository.AddAsync(code, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return code;
    }
}
