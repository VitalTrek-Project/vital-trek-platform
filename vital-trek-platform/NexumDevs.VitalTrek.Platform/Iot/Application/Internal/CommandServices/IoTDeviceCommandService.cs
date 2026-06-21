using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Iot.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.Internal.CommandServices;

public class IoTDeviceCommandService(
    IIoTDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IIoTDeviceCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<IoTDevice>> Handle(RegisterDeviceCommand command, CancellationToken cancellationToken)
    {
        var type = IoTEnumExtensions.ParseDeviceType(command.Type);
        var status = IoTEnumExtensions.ParseDeviceStatus(command.Status);
        if (type is null || status is null)
            return Result<IoTDevice>.Failure(IotError.InvalidDeviceType,
                $"Invalid Type or Status value. Received: Type='{command.Type}', Status='{command.Status}'");

        try
        {
            var device = new IoTDevice(command.Name, type.Value, status.Value, command.ExpeditionId, command.TouristId);
            await deviceRepository.AddAsync(device, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<IoTDevice>.Success(device);
        }
        catch (OperationCanceledException)
        {
            return Result<IoTDevice>.Failure(IotError.OperationCancelled, _localizer["OperationCancelled"]);
        }
        catch (DbUpdateException)
        {
            return Result<IoTDevice>.Failure(IotError.DatabaseError, _localizer["DatabaseError"]);
        }
        catch (Exception)
        {
            return Result<IoTDevice>.Failure(IotError.InternalServerError, _localizer["InternalServerError"]);
        }
    }

    public async Task<Result<IoTDevice>> Handle(RemoveDeviceCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var device = await deviceRepository.FindByIdAsync(command.DeviceId, cancellationToken);
            if (device is null)
                return Result<IoTDevice>.Failure(IotError.DeviceNotFound, _localizer["NotFound"]);

            deviceRepository.Remove(device);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<IoTDevice>.Success(device);
        }
        catch (OperationCanceledException)
        {
            return Result<IoTDevice>.Failure(IotError.OperationCancelled, _localizer["OperationCancelled"]);
        }
        catch (DbUpdateException)
        {
            return Result<IoTDevice>.Failure(IotError.DatabaseError, _localizer["DatabaseError"]);
        }
        catch (Exception)
        {
            return Result<IoTDevice>.Failure(IotError.InternalServerError, _localizer["InternalServerError"]);
        }
    }

    public async Task<Result<IoTDevice>> Handle(DispatchDeviceCommandCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var device = await deviceRepository.FindByIdAsync(command.DeviceId, cancellationToken);
            if (device is null)
                return Result<IoTDevice>.Failure(IotError.DeviceNotFound, _localizer["NotFound"]);

            device.DispatchCommand(command.CommandType, command.IssuedAt);
            deviceRepository.Update(device);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<IoTDevice>.Success(device);
        }
        catch (OperationCanceledException)
        {
            return Result<IoTDevice>.Failure(IotError.OperationCancelled, _localizer["OperationCancelled"]);
        }
        catch (DbUpdateException)
        {
            return Result<IoTDevice>.Failure(IotError.DatabaseError, _localizer["DatabaseError"]);
        }
        catch (Exception)
        {
            return Result<IoTDevice>.Failure(IotError.InternalServerError, _localizer["InternalServerError"]);
        }
    }
}