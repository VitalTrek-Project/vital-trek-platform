using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.QueryServices;

public class BinnacleReadingQueryService(
    IBinnacleReadingRepository binnacleReadingRepository)
    : IBinnacleReadingQueryService
{
    public async Task<IEnumerable<BinnacleReading>> Handle(
        GetBinnacleReadingsByExpeditionQuery query,
        CancellationToken cancellationToken)
    {
        return await binnacleReadingRepository
            .FindByExpeditionIdAsync(
                query.ExpeditionId,
                cancellationToken);
    }
}
