using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Alert endpoints")]
public class AlertsController(
    IAlertCommandService alertCommandService,
    IAlertQueryService alertQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Raise an alert",
        Description = "Raise a new alert for a tourist in an expedition",
        OperationId = "RaiseAlert")]
    [SwaggerResponse(StatusCodes.Status201Created, "The alert was created", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The alert was not created")]
    public async Task<IActionResult> RaiseAlert([FromBody] RaiseAlertResource resource, CancellationToken cancellationToken)
    {
        var command = new RaiseAlertCommand(resource.ExpeditionId, resource.TouristId, resource.Type, resource.Severity, resource.Message);
        var result = await alertCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, result.Error, result.Message);
        }
        var alertResource = AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetActiveAlertsByExpedition), new { expeditionId = alertResource.Id }, alertResource);
    }

    [HttpGet("expedition/{expeditionId:int}")]
    [SwaggerOperation(
        Summary = "Get active alerts by expedition",
        Description = "Get all active alerts for a specific expedition",
        OperationId = "GetActiveAlertsByExpedition")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alerts were found", typeof(IEnumerable<AlertResource>))]
    public async Task<IActionResult> GetActiveAlertsByExpedition([FromRoute] int expeditionId, CancellationToken cancellationToken)
    {
        var query = new GetActiveAlertsByExpeditionQuery(expeditionId);
        var alerts = await alertQueryService.Handle(query, cancellationToken);
        var alertResources = alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(alertResources);
    }

    [HttpPut("{alertId:int}/acknowledge")]
    [SwaggerOperation(
        Summary = "Acknowledge an alert",
        Description = "Acknowledge an active alert",
        OperationId = "AcknowledgeAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was acknowledged", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found")]
    public async Task<IActionResult> AcknowledgeAlert([FromRoute] int alertId, [FromQuery] int userId, CancellationToken cancellationToken)
    {
        var command = new AcknowledgeAlertCommand(alertId, userId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status404NotFound, result.Error, result.Message);
        }
        var alertResource = AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(alertResource);
    }

    [HttpPut("{alertId:int}/dismiss")]
    [SwaggerOperation(
        Summary = "Dismiss an alert",
        Description = "Dismiss an active or acknowledged alert",
        OperationId = "DismissAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was dismissed", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found")]
    public async Task<IActionResult> DismissAlert([FromRoute] int alertId, CancellationToken cancellationToken)
    {
        var command = new DismissAlertCommand(alertId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status404NotFound, result.Error, result.Message);
        }
        var alertResource = AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(alertResource);
    }
}