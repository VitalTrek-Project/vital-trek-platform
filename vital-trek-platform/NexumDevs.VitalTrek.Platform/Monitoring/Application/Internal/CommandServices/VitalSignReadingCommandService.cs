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

public class VitalSignReadingCommandService(
    IVitalSignReadingRepository vitalSignReadingRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IVitalSignReadingCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<VitalSignReading>> Handle(RecordVitalSignsCommand command, CancellationToken cancellationToken)
    {
        var vitalSignReading = new VitalSignReading(
            command.ExpeditionId,
            command.TouristId,
            command.HeartRate,
            command.BloodOxygen,
            command.BodyTemperature
        );
        try
        {
            await vitalSignReadingRepository.AddAsync(vitalSignReading, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<VitalSignReading>.Success(vitalSignReading);
        }
        catch (OperationCanceledException)
        {
            return Result<VitalSignReading>.Failure(MonitoringError.OperationCancelled,
                _localizer[nameof(MonitoringError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<VitalSignReading>.Failure(MonitoringError.DatabaseError,
                _localizer[nameof(MonitoringError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<VitalSignReading>.Failure(MonitoringError.InternalServerError,
                _localizer[nameof(MonitoringError.InternalServerError)]);
        }
    }
}