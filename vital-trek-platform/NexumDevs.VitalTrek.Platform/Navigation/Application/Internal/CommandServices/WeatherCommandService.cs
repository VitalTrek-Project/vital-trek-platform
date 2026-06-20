using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.CommandServices;

public class WeatherCommandService(
    IWeatherRepository weatherRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer,
    ILogger<WeatherCommandService> logger) 
    : IWeatherCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;
    
    public async Task<Result<Weather>> Handle(CreateWeatherCommand command, CancellationToken cancellationToken)
    {
        var weather = new Weather(command);

        try
        {
            await weatherRepository.AddAsync(weather, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Weather>.Success(weather);
        }
        catch (OperationCanceledException)
        {
            return Result<Weather>.Failure(NavigationError.OperationCancelled,
                _localizer[nameof(NavigationError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Weather>.Failure(NavigationError.DatabaseError,
                _localizer[nameof(NavigationError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Weather>.Failure(NavigationError.InternalServerError,
                _localizer[nameof(NavigationError.InternalServerError)]);
        }
    }
}
