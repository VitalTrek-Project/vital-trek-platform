using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.QueryServices;

public class WeatherQueryService(IWeatherRepository weatherRepository) : IWeatherQueryService
{
    public async Task<Weather?> Handle(GetWeatherByIdQuery query, CancellationToken cancellationToken)
    {
        return await weatherRepository.FindByIdAsync(query.WeatherId, cancellationToken);
    }
}
