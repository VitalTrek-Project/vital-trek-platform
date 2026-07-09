namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record CreateRewardCommand(Guid AgencyId, string Name, string Description, int PointsCost, int? Stock);
