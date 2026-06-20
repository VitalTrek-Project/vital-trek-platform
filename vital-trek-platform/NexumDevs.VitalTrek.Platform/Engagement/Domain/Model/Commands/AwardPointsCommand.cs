namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

/// <summary>
/// Command used to award points to a gamification profile
/// for a completed expedition.
/// </summary>
/// <param name="TouristId">The identifier of the tourist receiving the award.</param>
/// <param name="ExpeditionId">The identifier of the completed expedition.</param>
/// <param name="Points">The number of points to award.</param>
public record AwardPointsCommand(Guid TouristId, Guid ExpeditionId, int Points);
