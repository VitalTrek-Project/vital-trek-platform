using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root of the Engagement (Loyalty) Bounded Context.
/// Represents a tourist's loyalty standing within a single agency's program:
/// their points balance and current tier. A tourist has one profile per agency,
/// since points are not transferable between agencies.
/// </summary>
/// <remarks>
/// <see cref="TotalPoints"/> is a read-optimized cache, not the source of truth —
/// the <c>PointsTransaction</c> ledger is authoritative and immutable. This cache
/// is only ever mutated through <see cref="ApplyPointsDelta"/>, always alongside
/// the creation of a corresponding ledger transaction in the same unit of work.
/// </remarks>
public class GamificationProfile
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected GamificationProfile() { }

    /// <summary>
    /// Initializes a new loyalty profile for a tourist within a specific agency's program.
    /// </summary>
    /// <param name="touristId">The identifier of the tourist this profile belongs to.</param>
    /// <param name="agencyId">The identifier of the agency this profile's points belong to.</param>
    public GamificationProfile(Guid touristId, Guid agencyId)
    {
        Id = Guid.NewGuid();
        TouristId = touristId;
        AgencyId = agencyId;
        TotalPoints = 0;
        CurrentTierId = null;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>Gets the unique identifier of this gamification profile.</summary>
    public Guid Id { get; private set; }

    /// <summary>Gets the identifier of the tourist this profile belongs to.</summary>
    public Guid TouristId { get; private set; }

    /// <summary>Gets the identifier of the agency this profile's points belong to.</summary>
    public Guid AgencyId { get; private set; }

    /// <summary>Gets the cached points balance. Reconciled from the ledger, never edited directly.</summary>
    public int TotalPoints { get; private set; }

    /// <summary>Gets the identifier of the tier this profile currently qualifies for, if any.</summary>
    public Guid? CurrentTierId { get; private set; }

    /// <summary>Gets the date and time when the profile was created.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>Gets the date and time when the profile was last updated.</summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Applies a points delta to the cached balance (positive for earning, negative
    /// for redemption/expiration/adjustment), keeping it in sync with a ledger entry
    /// created in the same operation. Never allows the balance to go negative.
    /// </summary>
    /// <param name="delta">The signed points change to apply.</param>
    /// <exception cref="EngagementError">Thrown when the delta would make the balance negative.</exception>
    public void ApplyPointsDelta(int delta)
    {
        var newBalance = TotalPoints + delta;
        if (newBalance < 0)
            throw new EngagementError(EngagementErrors.InsufficientBalance);

        TotalPoints = newBalance;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Recalculates the current tier from the profile's balance against the agency's
    /// configured tiers (the highest tier whose <c>MinPoints</c> does not exceed the
    /// balance). Called after every points change, including expirations, so demotions
    /// are handled the same way as promotions.
    /// </summary>
    /// <param name="agencyTiersByMinPointsDescending">
    /// The agency's tiers, already ordered by <c>MinPoints</c> descending.
    /// </param>
    public void RecalculateTier(IReadOnlyList<LoyaltyTier> agencyTiersByMinPointsDescending)
    {
        var qualifyingTier = agencyTiersByMinPointsDescending.FirstOrDefault(tier => tier.MinPoints <= TotalPoints);
        CurrentTierId = qualifyingTier?.Id;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
