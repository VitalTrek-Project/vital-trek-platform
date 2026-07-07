using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root holding one agency's loyalty program configuration:
/// how many points each action is worth, and the points expiration policy.
/// </summary>
public class LoyaltyProgram
{
    private const int DefaultPointsPerExpeditionCompleted = 100;
    private const int DefaultPointsPerExpeditionBooked = 20;
    private const int DefaultPointsPerReferral = 50;
    private const int DefaultPointsPerReview = 30;

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected LoyaltyProgram() { }

    public LoyaltyProgram(
        Guid agencyId,
        int pointsPerExpeditionCompleted,
        int pointsPerExpeditionBooked,
        int pointsPerReferral,
        int pointsPerReview,
        int? expirationMonths)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdateSettings(pointsPerExpeditionCompleted, pointsPerExpeditionBooked, pointsPerReferral, pointsPerReview, expirationMonths);
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public int PointsPerExpeditionCompleted { get; private set; }
    public int PointsPerExpeditionBooked { get; private set; }
    public int PointsPerReferral { get; private set; }
    public int PointsPerReview { get; private set; }

    /// <summary>Months until earned points expire, or <c>null</c> if points never expire.</summary>
    public int? ExpirationMonths { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a program with sensible defaults for an agency that has not configured
    /// one yet (lazy initialization, same spirit as the profile auto-creation already
    /// used elsewhere in this bounded context).
    /// </summary>
    public static LoyaltyProgram CreateDefault(Guid agencyId) => new(
        agencyId,
        DefaultPointsPerExpeditionCompleted,
        DefaultPointsPerExpeditionBooked,
        DefaultPointsPerReferral,
        DefaultPointsPerReview,
        expirationMonths: null);

    public void UpdateSettings(
        int pointsPerExpeditionCompleted,
        int pointsPerExpeditionBooked,
        int pointsPerReferral,
        int pointsPerReview,
        int? expirationMonths)
    {
        if (pointsPerExpeditionCompleted < 0 || pointsPerExpeditionBooked < 0 || pointsPerReferral < 0 || pointsPerReview < 0)
            throw new EngagementError(EngagementErrors.InvalidProgramConfiguration);

        if (expirationMonths is <= 0)
            throw new EngagementError(EngagementErrors.InvalidProgramConfiguration);

        PointsPerExpeditionCompleted = pointsPerExpeditionCompleted;
        PointsPerExpeditionBooked = pointsPerExpeditionBooked;
        PointsPerReferral = pointsPerReferral;
        PointsPerReview = pointsPerReview;
        ExpirationMonths = expirationMonths;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Resolves how many points a given earning event is worth under this program.
    /// Only applies to event types with a configured per-action value — redemptions,
    /// expirations and manual adjustments compute their own points elsewhere.
    /// </summary>
    /// <exception cref="EngagementError">Thrown when <paramref name="type"/> has no configured value.</exception>
    public int ResolvePointsFor(PointsTransactionType type) => type switch
    {
        PointsTransactionType.ExpeditionCompleted => PointsPerExpeditionCompleted,
        PointsTransactionType.ExpeditionBooked => PointsPerExpeditionBooked,
        PointsTransactionType.ReferralBonus => PointsPerReferral,
        PointsTransactionType.ReviewSubmitted => PointsPerReview,
        _ => throw new EngagementError(EngagementErrors.InvalidEventType)
    };

    /// <summary>Computes the expiration timestamp for points earned right now, per this program's policy.</summary>
    public DateTimeOffset? ComputeExpiresAt() =>
        ExpirationMonths.HasValue ? DateTimeOffset.UtcNow.AddMonths(ExpirationMonths.Value) : null;
}
