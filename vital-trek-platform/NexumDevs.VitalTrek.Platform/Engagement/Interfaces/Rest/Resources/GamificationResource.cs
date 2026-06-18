namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents the gamification profile resource returned by the loyalty API.
/// </summary>
/// <param name="ProfileId">
/// The unique identifier of the tourist whose profile is represented.
/// </param>
/// <param name="TotalPoints">
/// The cumulative total of loyalty points earned by the tourist.
/// </param>
/// <param name="Rank">
/// The rank achieved by the tourist based on their total points.
/// </param>
/// <param name="UnlockedBadges">
/// The collection of badges unlocked by the tourist based on their total points.
/// </param>
public record GamificationResource(
    Guid ProfileId,
    int TotalPoints,
    string Rank,
    IEnumerable<string> UnlockedBadges);
