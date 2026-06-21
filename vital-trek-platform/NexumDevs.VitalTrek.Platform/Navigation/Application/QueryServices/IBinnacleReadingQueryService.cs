using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;

public interface IBinnacleReadingQueryService
{
    Task<IEnumerable<BinnacleReading>> Handle(GetBinnacleReadingsByExpeditionQuery query, CancellationToken cancellationToken);
}
