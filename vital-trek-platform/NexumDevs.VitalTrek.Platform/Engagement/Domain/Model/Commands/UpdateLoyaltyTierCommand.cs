namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record UpdateLoyaltyTierCommand(Guid AgencyId, Guid TierId, string Name, int MinPoints, string Benefits, int SortOrder);
