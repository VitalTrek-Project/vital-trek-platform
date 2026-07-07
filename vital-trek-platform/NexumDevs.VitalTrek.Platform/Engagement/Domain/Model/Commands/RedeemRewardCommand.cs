namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record RedeemRewardCommand(Guid AgencyId, Guid TouristId, Guid RewardId);
