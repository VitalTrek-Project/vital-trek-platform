using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

public interface IWeatherRepository : IBaseRepository<Weather>
{
    Task<IEnumerable<Weather>> FindExperienceByExpeditionIdAsync(int expeditionId, CancellationToken cancellationToken);
}
