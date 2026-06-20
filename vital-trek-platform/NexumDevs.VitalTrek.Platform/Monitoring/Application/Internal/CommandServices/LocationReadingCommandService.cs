using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.CommandServices;

public class LocationReadingCommandService(
    ILocationReadingRepository locationReadingRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : ILocationReadingCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<LocationReading>> Handle(RecordLocationCommand command, CancellationToken cancellationToken)
    {
        var locationReading = new LocationReading(
            command.ExpeditionId,
            command.TouristId,
            command.Latitude,
            command.Longitude,
            command.AccuracyMeters
        );
        try
        {
            await locationReadingRepository.AddAsync(locationReading, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<LocationReading>.Success(locationReading);
        }
        catch (OperationCanceledException)
        {
            return Result<LocationReading>.Failure(MonitoringError.OperationCancelled,
                _localizer[nameof(MonitoringError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<LocationReading>.Failure(MonitoringError.DatabaseError,
                _localizer[nameof(MonitoringError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<LocationReading>.Failure(MonitoringError.InternalServerError,
                _localizer[nameof(MonitoringError.InternalServerError)]);
        }
    }
}