using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;

public interface IRedemptionRepository : IBaseRepository<Redemption>
{
    Task<IReadOnlyList<Redemption>> FindByTouristAsync(Guid agencyId, Guid touristId, CancellationToken cancellationToken);

    Task<Redemption?> FindByCodeAsync(Guid agencyId, string code, CancellationToken cancellationToken);

    Task<Redemption?> FindByIdAndAgencyAsync(Guid redemptionId, Guid agencyId, CancellationToken cancellationToken);
}
