using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

/// <summary>
/// Records that a referral code was redeemed by a new tourist. The referral stays
/// Pending — and the referrer earns nothing — until the referred tourist completes
/// their first expedition (see <see cref="ProfileCommandService"/>).
/// </summary>
public class ReferralCommandService(
    IReferralCodeRepository referralCodeRepository,
    IReferralRepository referralRepository,
    IUnitOfWork unitOfWork) : IReferralCommandService
{
    public async Task<Referral> Handle(RedeemReferralCodeCommand command, CancellationToken cancellationToken)
    {
        var referralCode = await referralCodeRepository.FindByCodeAsync(command.AgencyId, command.Code, cancellationToken)
                            ?? throw new EngagementError(EngagementErrors.ReferralCodeNotFound);

        var existingPending = await referralRepository.FindPendingByReferredTouristAsync(command.AgencyId, command.ReferredTouristId, cancellationToken);
        if (existingPending is not null)
            throw new EngagementError(EngagementErrors.ReferralAlreadyUsed);

        var referral = new Referral(command.AgencyId, referralCode.Id, referralCode.TouristId, command.ReferredTouristId);
        await referralRepository.AddAsync(referral, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return referral;
    }
}
