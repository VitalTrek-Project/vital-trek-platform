using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Iot.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iot.Application.Internal.CommandServices;

public class SensorReadingCommandService(
    ISensorReadingRepository readingRepository,
    IIoTDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : ISensorReadingCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<SensorReading>> Handle(RecordSensorReadingCommand command, CancellationToken cancellationToken)
    {
        var sensorType = IoTEnumExtensions.ParseSensorType(command.Type);
        if (sensorType is null)
            return Result<SensorReading>.Failure(IotError.InvalidSensorType,
                $"Invalid sensor type. Received: '{command.Type}'");

        // Previously unchecked: a reading for a nonexistent device only failed via the DB's FK
        // constraint, surfacing as a generic DatabaseError/400 instead of a clean 404.
        var device = await deviceRepository.FindByIdAsync(command.DeviceId, cancellationToken);
        if (device is null)
            return Result<SensorReading>.Failure(IotError.DeviceNotFound,
                $"No device found with id '{command.DeviceId}'.");

        try
        {
            var reading = new SensorReading(command.DeviceId, sensorType.Value, command.Value, command.Unit, command.RecordedAt);
            await readingRepository.AddAsync(reading, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SensorReading>.Success(reading);
        }
        catch (OperationCanceledException)
        {
            return Result<SensorReading>.Failure(IotError.OperationCancelled, _localizer["OperationCancelled"]);
        }
        catch (DbUpdateException)
        {
            return Result<SensorReading>.Failure(IotError.DatabaseError, _localizer["DatabaseError"]);
        }
        catch (Exception)
        {
            return Result<SensorReading>.Failure(IotError.InternalServerError, _localizer["InternalServerError"]);
        }
    }
}