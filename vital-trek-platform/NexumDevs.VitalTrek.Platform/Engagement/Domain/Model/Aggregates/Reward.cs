using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing one item in an agency's redeemable rewards catalog.
/// </summary>
public class Reward
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Reward() { }

    public Reward(Guid agencyId, string name, string description, int pointsCost, int? stock)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        Update(name, description, pointsCost, stock);
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int PointsCost { get; private set; }

    /// <summary>Remaining redeemable units, or <c>null</c> for unlimited stock.</summary>
    public int? Stock { get; private set; }

    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public void Update(string name, string description, int pointsCost, int? stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EngagementError(EngagementErrors.RewardNotFound, "Reward name is required.");

        if (pointsCost <= 0)
            throw new EngagementError(EngagementErrors.InvalidPoints);

        if (stock is < 0)
            throw new EngagementError(EngagementErrors.InsufficientStock);

        Name = name;
        Description = description ?? string.Empty;
        PointsCost = pointsCost;
        Stock = stock;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reserves one unit of stock for a redemption. No-op for unlimited-stock rewards.
    /// </summary>
    /// <exception cref="EngagementError">
    /// Thrown when the reward is inactive or has no remaining stock.
    /// </exception>
    public void ReserveForRedemption()
    {
        if (!IsActive)
            throw new EngagementError(EngagementErrors.RewardInactive);

        if (Stock is 0)
            throw new EngagementError(EngagementErrors.InsufficientStock);

        if (Stock.HasValue)
        {
            Stock -= 1;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
