namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

/// <summary>
/// Query used to retrieve the gamification profile for a specific tourist.
/// </summary>
/// <param name="TouristId">The identifier of the tourist whose profile is requested.</param>
public record GetGamificationProfileQuery(Guid TouristId);
