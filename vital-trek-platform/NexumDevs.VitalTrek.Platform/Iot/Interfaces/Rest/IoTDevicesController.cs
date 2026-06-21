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
[Route("api/v1/devices")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available IoT Device endpoints")]
public class IoTDevicesController(
    IIoTDeviceCommandService deviceCommandService,
    IIoTDeviceQueryService deviceQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Register a device",
        Description = "Register a new IoT device",
        OperationId = "RegisterDevice")]
    [SwaggerResponse(StatusCodes.Status201Created, "The device was registered", typeof(DeviceResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The device was not registered")]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceResource resource, CancellationToken cancellationToken)
    {
        var command = new RegisterDeviceCommand(resource.Name, resource.Type, resource.Status, resource.ExpeditionId, resource.TouristId);
        var result = await deviceCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, result.Error, result.Message);
        }
        var deviceResource = DeviceResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetDeviceById), new { deviceId = deviceResource.Id }, deviceResource);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all devices",
        Description = "Get all registered IoT devices",
        OperationId = "GetAllDevices")]
    [SwaggerResponse(StatusCodes.Status200OK, "Devices retrieved", typeof(IEnumerable<DeviceResource>))]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        var query = new GetAllDevicesQuery();
        var devices = await deviceQueryService.Handle(query, cancellationToken);
        var resources = devices.Select(DeviceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{deviceId:int}")]
    [SwaggerOperation(
        Summary = "Get device by ID",
        Description = "Get a specific IoT device by its ID",
        OperationId = "GetDeviceById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Device found", typeof(DeviceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found")]
    public async Task<IActionResult> GetDeviceById([FromRoute] int deviceId, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(deviceId);
        var device = await deviceQueryService.Handle(query, cancellationToken);
        if (device is null)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status404NotFound,
                IotError.DeviceNotFound, _errorLocalizer["DeviceNotFound"]);
        }
        return Ok(DeviceResourceFromEntityAssembler.ToResourceFromEntity(device));
    }

    [HttpDelete("{deviceId:int}")]
    [SwaggerOperation(
        Summary = "Remove a device",
        Description = "Remove an IoT device by its ID",
        OperationId = "RemoveDevice")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Device removed")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found")]
    public async Task<IActionResult> RemoveDevice([FromRoute] int deviceId, CancellationToken cancellationToken)
    {
        var command = new RemoveDeviceCommand(deviceId);
        var result = await deviceCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status404NotFound, result.Error, result.Message);
        }
        return NoContent();
    }

    // PUT /{deviceId} — matches frontend: this.#devicesEndpoint.update(deviceId, {...resource, lastCommand, lastSeen})
    [HttpPut("{deviceId:int}")]
    [SwaggerOperation(
        Summary = "Dispatch a command to a device",
        Description = "Send a command to a specific IoT device. Accepts full device resource merged with lastCommand and lastSeen; extra fields are ignored.",
        OperationId = "DispatchDeviceCommand")]
    [SwaggerResponse(StatusCodes.Status200OK, "Command dispatched", typeof(DeviceResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid command")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found")]
    public async Task<IActionResult> DispatchDeviceCommand([FromRoute] int deviceId, [FromBody] DispatchCommandResource resource, CancellationToken cancellationToken)
    {
        var command = new DispatchDeviceCommandCommand(deviceId, resource.LastCommand ?? string.Empty, resource.LastSeen ?? DateTime.UtcNow);
        var result = await deviceCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            var statusCode = result.Error is IotError.DeviceNotFound
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;
            return _problemDetailsFactory.CreateProblemDetails(this, statusCode, result.Error, result.Message);
        }
        return Ok(DeviceResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }
}