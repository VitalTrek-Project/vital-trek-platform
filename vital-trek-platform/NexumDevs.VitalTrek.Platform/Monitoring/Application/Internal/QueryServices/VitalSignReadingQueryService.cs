using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.QueryServices;

public class VitalSignReadingQueryService(IVitalSignReadingRepository vitalSignReadingRepository) : IVitalSignReadingQueryService
{
    public async Task<IEnumerable<VitalSignReading>> Handle(GetVitalSignReadingsByExpeditionQuery query, CancellationToken cancellationToken)
    {
        return await vitalSignReadingRepository.FindByExpeditionIdAsync(query.ExpeditionId, cancellationToken);
    }
}