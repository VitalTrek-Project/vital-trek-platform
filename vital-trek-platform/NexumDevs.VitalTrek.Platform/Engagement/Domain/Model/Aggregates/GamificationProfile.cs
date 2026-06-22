using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root of the Engagement Bounded Context.
/// Represents the gamification profile of a tourist, tracking their
/// total points, earned rank, and unlocked badges across all expeditions..
/// </summary>
public class GamificationProfile
{
    private readonly List<AwardedExpedition> _awardedExpeditions = new();

    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected GamificationProfile() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GamificationProfile"/> class.
    /// </summary>
    /// <param name="touristId">The identifier of the tourist this profile belongs to.</param>
    public GamificationProfile(Guid touristId)
    {
        Id = Guid.NewGuid();
        TouristId = touristId;
        TotalPoints = 0;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of this gamification profile.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the tourist this profile belongs to.
    /// </summary>
    public Guid TouristId { get; private set; }

    /// <summary>
    /// Gets the cumulative total of points earned across all expeditions.
    /// </summary>
    public int TotalPoints { get; private set; }

    /// <summary>
    /// Gets the date and time when the profile was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the profile was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the collection of expeditions for which points have been awarded.
    /// </summary>
    public IReadOnlyCollection<AwardedExpedition> AwardedExpeditions => _awardedExpeditions.AsReadOnly();

    /// <summary>
    /// Gets the rank of the tourist based on their total accumulated points.
    /// </summary>
    public string Rank => ComputeRank(TotalPoints);

    /// <summary>
    /// Gets the list of badges unlocked by the tourist based on their total points.
    /// </summary>
    public IReadOnlyCollection<string> UnlockedBadges => ComputeBadges(TotalPoints);

    /// <summary>
    /// Awards points to the profile for a completed expedition.
    /// Validates that the points are positive and the expedition has not been awarded before.
    /// Also triggers automatic badge evaluation based on the updated total.
    /// </summary>
    /// <param name="expeditionId">The identifier of the completed expedition.</param>
    /// <param name="points">The number of points to award. Must be greater than zero.</param>
    /// <exception cref="EngagementError">
    /// Thrown when points are less than or equal to zero, or when the expedition
    /// has already been awarded to this profile.
    /// </exception>
    public void AwardPoints(Guid expeditionId, int points)
    {
        if (points <= 0)
            throw new EngagementError(EngagementErrors.InvalidPoints, "Points must be greater than zero.");

        if (_awardedExpeditions.Any(e => e.ExpeditionId == expeditionId))
            throw new EngagementError(EngagementErrors.ExpeditionAlreadyAwarded, "This expedition has already been awarded to this profile.");

        var award = new AwardedExpedition(Id, expeditionId, points);
        _awardedExpeditions.Add(award);
        TotalPoints += points;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static string ComputeRank(int totalPoints) => totalPoints switch
    {
        < 100 => "Novice",
        < 500 => "Explorer",
        < 1000 => "Adventurer",
        < 5000 => "Elite",
        _ => "Legend"
    };

    private static IReadOnlyCollection<string> ComputeBadges(int totalPoints)
    {
        var badges = new List<string>();
        if (totalPoints >= 100) badges.Add("Bronze");
        if (totalPoints >= 500) badges.Add("Silver");
        if (totalPoints >= 1000) badges.Add("Gold");
        if (totalPoints >= 5000) badges.Add("Platinum");
        return badges.AsReadOnly();
    }
}
