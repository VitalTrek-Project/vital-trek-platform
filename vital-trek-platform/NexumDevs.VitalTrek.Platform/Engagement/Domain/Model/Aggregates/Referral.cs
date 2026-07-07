using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root tracking one referred tourist's usage of a <see cref="ReferralCode"/>.
/// Stays <see cref="ReferralStatus.Pending"/> until the referred tourist completes their
/// first expedition — only then is the referrer awarded points, to discourage abuse via
/// sign-up-only referrals.
/// </summary>
public class Referral
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Referral() { }

    public Referral(Guid agencyId, Guid referralCodeId, Guid referrerTouristId, Guid referredTouristId)
    {
        if (referrerTouristId == referredTouristId)
            throw new EngagementError(EngagementErrors.SelfReferralNotAllowed);

        Id = Guid.NewGuid();
        AgencyId = agencyId;
        ReferralCodeId = referralCodeId;
        ReferrerTouristId = referrerTouristId;
        ReferredTouristId = referredTouristId;
        Status = ReferralStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public Guid ReferralCodeId { get; private set; }
    public Guid ReferrerTouristId { get; private set; }
    public Guid ReferredTouristId { get; private set; }
    public ReferralStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// Marks the referral as completed once the referred tourist finishes their
    /// first expedition, unlocking the referrer's bonus.
    /// </summary>
    /// <exception cref="EngagementError">Thrown when the referral was already completed.</exception>
    public void Complete()
    {
        if (Status != ReferralStatus.Pending)
            throw new EngagementError(EngagementErrors.ReferralAlreadyUsed);

        Status = ReferralStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
