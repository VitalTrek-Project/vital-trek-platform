using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;

public interface IProgramQueryService
{
    /// <summary>Lazily provisions a default program for the agency on first access.</summary>
    Task<LoyaltyProgram> Handle(GetLoyaltyProgramQuery query, CancellationToken cancellationToken);

    /// <summary>Lazily seeds the default tiers for the agency on first access.</summary>
    Task<IReadOnlyList<LoyaltyTier>> Handle(GetLoyaltyTiersQuery query, CancellationToken cancellationToken);
}
