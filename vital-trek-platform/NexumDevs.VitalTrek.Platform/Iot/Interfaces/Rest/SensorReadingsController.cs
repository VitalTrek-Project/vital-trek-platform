using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NexumDevs.VitalTrek.Platform.Iot.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Iot.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest;

[ApiController]
[Route("api/v1/sensor-readings")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Sensor Reading endpoints")]
public class SensorReadingsController(
    ISensorReadingCommandService sensorReadingCommandService,
    ISensorReadingQueryService sensorReadingQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Record a sensor reading",
        Description = "Record a new sensor reading from an IoT device",
        OperationId = "RecordSensorReading")]
    [SwaggerResponse(StatusCodes.Status201Created, "The sensor reading was recorded", typeof(SensorReadingResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The sensor reading was not recorded")]
    public async Task<IActionResult> RecordSensorReading([FromBody] RecordSensorReadingResource resource, CancellationToken cancellationToken)
    {
        var command = new RecordSensorReadingCommand(resource.DeviceId, resource.Type, resource.Value, resource.Unit, resource.RecordedAt);
        var result = await sensorReadingCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            var statusCode = (IotError)result.Error! == IotError.DeviceNotFound
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;

            return _problemDetailsFactory.CreateProblemDetails(this, statusCode, result.Error, result.Message);
        }
        var readingResource = SensorReadingResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetSensorReadingsByDevice), new { deviceId = readingResource.DeviceId }, readingResource);
    }

    // GET / — frontend calls getAll() and filters in memory by deviceId
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all sensor readings",
        Description = "Get all sensor readings. The frontend filters by deviceId in memory.",
        OperationId = "GetAllSensorReadings")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sensor readings retrieved", typeof(IEnumerable<SensorReadingResource>))]
    public async Task<IActionResult> GetAllSensorReadings(CancellationToken cancellationToken)
    {
        var query = new GetAllSensorReadingsQuery();
        var readings = await sensorReadingQueryService.Handle(query, cancellationToken);
        var resources = readings.Select(SensorReadingResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("/api/v1/devices/{deviceId:int}/sensor-readings")]
    [SwaggerOperation(
        Summary = "Get sensor readings by device",
        Description = "Get all sensor readings for a specific IoT device",
        OperationId = "GetSensorReadingsByDevice")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sensor readings retrieved", typeof(IEnumerable<SensorReadingResource>))]
    public async Task<IActionResult> GetSensorReadingsByDevice([FromRoute] int deviceId, CancellationToken cancellationToken)
    {
        var query = new GetSensorReadingsByDeviceQuery(deviceId);
        var readings = await sensorReadingQueryService.Handle(query, cancellationToken);
        var resources = readings.Select(SensorReadingResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}