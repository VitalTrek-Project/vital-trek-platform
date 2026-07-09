namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;

/// <summary>
/// Records that a tourist earned a specific badge under a specific agency's program.
/// </summary>
public class AwardedBadge
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected AwardedBadge() { }

    public AwardedBadge(Guid agencyId, Guid touristId, Guid badgeDefinitionId)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        TouristId = touristId;
        BadgeDefinitionId = badgeDefinitionId;
        AwardedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public Guid TouristId { get; private set; }
    public Guid BadgeDefinitionId { get; private set; }
    public DateTimeOffset AwardedAt { get; private set; }
}
