namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;

/// <summary>
/// Represents a record of points awarded to a tourist for completing
/// a specific expedition. Used to track which expeditions have already
/// been rewarded, preventing duplicate awards.
/// </summary>
public class AwardedExpedition
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected AwardedExpedition() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AwardedExpedition"/> class.
    /// </summary>
    /// <param name="profileId">The identifier of the parent gamification profile.</param>
    /// <param name="expeditionId">The identifier of the expedition being awarded.</param>
    /// <param name="points">The number of points earned for this expedition.</param>
    public AwardedExpedition(Guid profileId, Guid expeditionId, int points)
    {
        Id = Guid.NewGuid();
        ProfileId = profileId;
        ExpeditionId = expeditionId;
        Points = points;
        AwardedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of this award record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the parent gamification profile.
    /// </summary>
    public Guid ProfileId { get; private set; }

    /// <summary>
    /// Gets the identifier of the expedition that was awarded.
    /// </summary>
    public Guid ExpeditionId { get; private set; }

    /// <summary>
    /// Gets the number of points earned for this expedition.
    /// </summary>
    public int Points { get; private set; }

    /// <summary>
    /// Gets the date and time when the award was granted.
    /// </summary>
    public DateTimeOffset AwardedAt { get; private set; }
}
