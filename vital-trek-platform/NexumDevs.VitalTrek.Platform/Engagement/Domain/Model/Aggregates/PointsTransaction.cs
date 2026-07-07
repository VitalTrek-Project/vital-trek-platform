using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing a single, immutable entry in a tourist's points ledger.
/// This is the authoritative source of truth for a profile's balance — rows are never
/// updated or deleted, only appended. Idempotency for repeatable-source events (an
/// expedition being completed, a review being submitted, etc.) is enforced by a unique
/// index on (TouristId, AgencyId, Type, SourceId).
/// </summary>
public class PointsTransaction
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected PointsTransaction() { }

    /// <summary>
    /// Records a new ledger entry.
    /// </summary>
    /// <param name="profileId">The gamification profile this entry belongs to.</param>
    /// <param name="agencyId">The agency this entry's points belong to.</param>
    /// <param name="touristId">The tourist this entry belongs to.</param>
    /// <param name="type">The kind of event that produced this entry.</param>
    /// <param name="points">
    /// The signed points delta. Positive for earning events, negative for redemptions,
    /// expirations, or downward manual adjustments.
    /// </param>
    /// <param name="sourceId">
    /// An opaque reference to whatever triggered this entry (an expedition id, a review
    /// id, a redemption id...). Deliberately a string rather than a single typed FK,
    /// since sources come from bounded contexts with incompatible id types (Navigation's
    /// Expedition.Id is <c>int</c>, everything in this bounded context is <c>Guid</c>).
    /// </param>
    /// <param name="description">A human-readable description shown in the tourist's history.</param>
    /// <param name="expiresAt">
    /// When these points expire per the agency's policy, or <c>null</c> if they never
    /// expire or this entry does not represent earned points.
    /// </param>
    /// <exception cref="EngagementError">Thrown when <paramref name="points"/> is zero.</exception>
    public PointsTransaction(
        Guid profileId,
        Guid agencyId,
        Guid touristId,
        PointsTransactionType type,
        int points,
        string? sourceId,
        string description,
        DateTimeOffset? expiresAt)
    {
        if (points == 0)
            throw new EngagementError(EngagementErrors.InvalidPoints);

        Id = Guid.NewGuid();
        ProfileId = profileId;
        AgencyId = agencyId;
        TouristId = touristId;
        Type = type;
        Points = points;
        SourceId = sourceId;
        Description = description;
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid ProfileId { get; private set; }
    public Guid AgencyId { get; private set; }
    public Guid TouristId { get; private set; }
    public PointsTransactionType Type { get; private set; }
    public int Points { get; private set; }
    public string? SourceId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTimeOffset? ExpiresAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
