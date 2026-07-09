namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record DeleteLoyaltyTierCommand(Guid AgencyId, Guid TierId);
