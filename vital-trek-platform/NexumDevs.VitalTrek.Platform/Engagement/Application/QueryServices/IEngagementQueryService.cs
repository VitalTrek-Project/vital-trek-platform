using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

/// <summary>
/// Defines the contract for handling engagement-related queries,
/// including retrieving a tourist's gamification profile.
/// </summary>
public interface IEngagementQueryService
{
    /// <summary>
    /// Retrieves the gamification profile for a specific tourist.
    /// </summary>
    /// <param name="query">The query containing the tourist identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="GamificationProfile"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<GamificationProfile?> Handle(GetGamificationProfileQuery query, CancellationToken cancellationToken);
}
