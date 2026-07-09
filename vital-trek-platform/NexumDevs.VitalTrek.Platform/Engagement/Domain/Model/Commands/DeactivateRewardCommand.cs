namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

/// <summary>
/// Backs the REST "delete" endpoint for a reward. Deactivates rather than hard-deletes,
/// since past redemptions keep a reference to the reward and must remain intact.
/// </summary>
public record DeactivateRewardCommand(Guid AgencyId, Guid RewardId);
