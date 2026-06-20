using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.QueryServices;

public class LocationReadingQueryService(ILocationReadingRepository locationReadingRepository) : ILocationReadingQueryService
{
    public async Task<IEnumerable<LocationReading>> Handle(GetLocationReadingsByExpeditionQuery query, CancellationToken cancellationToken)
    {
        return await locationReadingRepository.FindByExpeditionIdAsync(query.ExpeditionId, cancellationToken);
    }
}