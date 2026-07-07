namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

public record GetRewardsQuery(Guid AgencyId, bool? ActiveOnly);
