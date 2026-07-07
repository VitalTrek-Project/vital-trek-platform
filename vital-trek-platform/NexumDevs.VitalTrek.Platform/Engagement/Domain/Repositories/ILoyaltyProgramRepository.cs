using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface ILoyaltyProgramRepository : IBaseRepository<LoyaltyProgram>
{
    Task<LoyaltyProgram?> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken);
}
