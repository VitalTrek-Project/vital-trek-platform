using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing one configurable loyalty tier within an agency's
/// program (e.g. "Explorador" at 0 points, "Trekker Elite" at 2000 points).
/// Benefits are descriptive only — no discount is applied automatically.
/// </summary>
public class LoyaltyTier
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected LoyaltyTier() { }

    public LoyaltyTier(Guid agencyId, string name, int minPoints, string benefits, int sortOrder)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        CreatedAt = DateTimeOffset.UtcNow;
        Update(name, minPoints, benefits, sortOrder);
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int MinPoints { get; private set; }
    public string Benefits { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public void Update(string name, int minPoints, string benefits, int sortOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EngagementError(EngagementErrors.InvalidTierConfiguration);

        if (minPoints < 0)
            throw new EngagementError(EngagementErrors.InvalidTierConfiguration);

        Name = name;
        MinPoints = minPoints;
        Benefits = benefits ?? string.Empty;
        SortOrder = sortOrder;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>Default tiers seeded when an agency's program is first created.</summary>
    public static IEnumerable<LoyaltyTier> CreateDefaults(Guid agencyId) =>
    [
        new(agencyId, "Explorador", 0, "Bienvenida al programa de fidelización.", 0),
        new(agencyId, "Aventurero", 500, "10% de descuento en accesorios de tienda.", 1),
        new(agencyId, "Trekker Elite", 2000, "Acceso prioritario y 15% de descuento.", 2)
    ];
}
