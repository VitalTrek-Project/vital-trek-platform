using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

/// <summary>
/// Defines the contract for handling engagement-related commands,
/// including awarding points to a gamification profile.
/// </summary>
public interface IEngagementCommandService
{
    /// <summary>
    /// Awards points to a tourist's gamification profile for a completed expedition.
    /// Creates the profile if it does not yet exist.
    /// </summary>
    /// <param name="command">The command containing the tourist identifier, expedition identifier, and points to award.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The updated <see cref="GamificationProfile"/>.</returns>
    Task<GamificationProfile> Handle(AwardPointsCommand command, CancellationToken cancellationToken);
}
