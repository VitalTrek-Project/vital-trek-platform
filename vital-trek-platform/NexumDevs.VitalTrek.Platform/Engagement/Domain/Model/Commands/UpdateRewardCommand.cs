namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record UpdateRewardCommand(Guid AgencyId, Guid RewardId, string Name, string Description, int PointsCost, int? Stock, bool IsActive);
