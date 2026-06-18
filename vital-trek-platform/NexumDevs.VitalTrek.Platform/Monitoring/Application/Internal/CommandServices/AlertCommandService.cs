using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.CommandServices;

public class AlertCommandService(
    IAlertRepository alertRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IAlertCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Alert>> Handle(RaiseAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = new Alert(command);
        try
        {
            await alertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return Result<Alert>.Failure(MonitoringError.OperationCancelled,
                _localizer["OperationCancelled"]);
        }
        catch (DbUpdateException)
        {
            return Result<Alert>.Failure(MonitoringError.DatabaseError,
                _localizer["DatabaseError"]);
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(MonitoringError.InternalServerError,
                _localizer["InternalServerError"]);
        }
    }

    public async Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
            if (alert == null)
            {
                return Result<Alert>.Failure(MonitoringError.AlertNotFound,
                    _localizer["NotFound"]);
            }

            alert.Acknowledge(command.UserId);
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(MonitoringError.InternalServerError,
                _localizer["InternalServerError"]);
        }
    }

    public async Task<Result<Alert>> Handle(DismissAlertCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
            if (alert == null)
            {
                return Result<Alert>.Failure(MonitoringError.AlertNotFound,
                    _localizer["NotFound"]);
            }

            alert.Dismiss();
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(MonitoringError.InternalServerError,
                _localizer["InternalServerError"]);
        }
    }
}