using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root holding a minimal expedition review — just enough to drive the
/// "leave a review" points event and prevent duplicates. Not a full review/moderation
/// system (out of scope); <see cref="ExpeditionId"/> is <c>int</c> because it references
/// Navigation's <c>Expedition.Id</c> directly (no real cross-context FK exists in this
/// codebase, same loose-reference convention already used by Monitoring/Navigation).
/// </summary>
public class Review
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Review() { }

    public Review(Guid agencyId, Guid touristId, int expeditionId, int rating, string? comment)
    {
        if (rating is < 1 or > 5)
            throw new EngagementError(EngagementErrors.InvalidReview);

        Id = Guid.NewGuid();
        AgencyId = agencyId;
        TouristId = touristId;
        ExpeditionId = expeditionId;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public Guid TouristId { get; private set; }
    public int ExpeditionId { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
