using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

/// <summary>
/// Query service responsible for handling engagement-related read operations,
/// including retrieving a tourist's gamification profile.
/// </summary>
public class EngagementQueryService : IEngagementQueryService
{
    private readonly IGamificationProfileRepository _profileRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="EngagementQueryService"/> class.
    /// </summary>
    /// <param name="profileRepository">Repository used to retrieve gamification profiles.</param>
    public EngagementQueryService(IGamificationProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    /// <summary>
    /// Retrieves the gamification profile for a specific tourist.
    /// Returns <c>null</c> when the tourist has not yet participated in any expedition.
    /// </summary>
    /// <param name="query">The query containing the tourist identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="GamificationProfile"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<GamificationProfile?> Handle(GetGamificationProfileQuery query, CancellationToken cancellationToken)
    {
        return await _profileRepository.FindByTouristIdAsync(query.TouristId, cancellationToken);
    }
}
