namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record CreateLoyaltyTierCommand(Guid AgencyId, string Name, int MinPoints, string Benefits, int SortOrder);
