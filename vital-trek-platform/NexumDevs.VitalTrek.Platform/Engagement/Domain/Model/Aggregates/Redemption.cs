using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Services;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing one instance of a tourist redeeming a <see cref="Reward"/>.
/// Carries its own redemption code so an agency admin can look it up and mark it used
/// in person (e.g. when the tourist shows up to claim a physical reward).
/// </summary>
public class Redemption
{
    private static readonly TimeSpan DefaultValidity = TimeSpan.FromDays(90);

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Redemption() { }

    public Redemption(Guid agencyId, Guid touristId, Guid rewardId, int pointsSpent)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        TouristId = touristId;
        RewardId = rewardId;
        PointsSpent = pointsSpent;
        Code = CodeGenerator.Generate();
        Status = RedemptionStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
        ExpiresAt = CreatedAt.Add(DefaultValidity);
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public Guid TouristId { get; private set; }
    public Guid RewardId { get; private set; }
    public int PointsSpent { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public RedemptionStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }

    /// <summary>
    /// Marks the redemption as used by an agency admin.
    /// </summary>
    /// <exception cref="EngagementError">
    /// Thrown when the redemption was already used, or has expired.
    /// </exception>
    public void MarkAsUsed()
    {
        if (ExpiresAt.HasValue && ExpiresAt.Value < DateTimeOffset.UtcNow && Status == RedemptionStatus.Pending)
            Status = RedemptionStatus.Expired;

        if (Status != RedemptionStatus.Pending)
            throw new EngagementError(EngagementErrors.RedemptionAlreadyProcessed);

        Status = RedemptionStatus.Used;
        UsedAt = DateTimeOffset.UtcNow;
    }
}
