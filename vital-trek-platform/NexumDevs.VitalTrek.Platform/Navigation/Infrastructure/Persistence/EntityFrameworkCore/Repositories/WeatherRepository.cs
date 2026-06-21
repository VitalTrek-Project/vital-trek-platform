using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class WeatherRepository(AppDbContext context) : BaseRepository<Weather>(context), IWeatherRepository
{
    public async Task<IEnumerable<Weather>> FindExperienceByExpeditionIdAsync(int expeditionId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<Weather>()
            .Include(weather => weather.Expedition)
            .Where(weather => weather.Expedition.Id == expeditionId)
            .ToListAsync(cancellationToken);
    }
}
