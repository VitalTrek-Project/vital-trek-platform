namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

public record FindRedemptionByCodeQuery(Guid AgencyId, string Code);
