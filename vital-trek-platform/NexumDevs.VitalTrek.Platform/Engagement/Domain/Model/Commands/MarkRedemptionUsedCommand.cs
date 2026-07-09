namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record MarkRedemptionUsedCommand(Guid AgencyId, Guid RedemptionId);
