using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

/// <summary>
/// Defines the repository contract for managing
/// <see cref="GamificationProfile"/> aggregates.
/// </summary>
public interface IGamificationProfileRepository : IBaseRepository<GamificationProfile>
{
    /// <summary>
    /// Retrieves the gamification profile associated with a specific tourist,
    /// including all awarded expeditions.
    /// </summary>
    /// <param name="touristId">The identifier of the tourist.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="GamificationProfile"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<GamificationProfile?> FindByTouristIdAsync(Guid touristId, CancellationToken cancellationToken);
}
