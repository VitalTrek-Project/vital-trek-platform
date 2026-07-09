using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
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

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this, result, _problemDetailsFactory,
            alert =>
            {
                var alertResource = AlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
                return CreatedAtAction(nameof(GetActiveAlertsByExpedition), new { expeditionId = alertResource.Id }, alertResource);
            });
    }

    [HttpGet("/api/v1/expeditions/{expeditionId:int}/alerts")]
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

    /// <summary>
    /// Updates an alert's status. Replaces the old PUT .../acknowledge and PUT .../dismiss
    /// action-suffixed routes with a single state-change PATCH on the alert resource itself.
    /// </summary>
    [HttpPatch("{alertId:int}")]
    [SwaggerOperation(
        Summary = "Update an alert's status",
        Description = "Set an alert's status to ACKNOWLEDGED (requires userId) or DISMISSED",
        OperationId = "UpdateAlertStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was updated", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid status or missing userId for ACKNOWLEDGED")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found")]
    public async Task<IActionResult> UpdateAlertStatus(
        [FromRoute] int alertId, [FromBody] UpdateAlertStatusResource resource, CancellationToken cancellationToken)
    {
        Result<Alert> result;
        switch (resource.Status.ToUpperInvariant())
        {
            case "ACKNOWLEDGED":
                if (resource.UserId is not { } userId)
                    return _problemDetailsFactory.CreateProblemDetails(
                        this, StatusCodes.Status400BadRequest, (Enum?)null,
                        "userId is required when setting status to ACKNOWLEDGED.");
                result = await alertCommandService.Handle(new AcknowledgeAlertCommand(alertId, userId), cancellationToken);
                break;
            case "DISMISSED":
                result = await alertCommandService.Handle(new DismissAlertCommand(alertId), cancellationToken);
                break;
            default:
                return _problemDetailsFactory.CreateProblemDetails(
                    this, StatusCodes.Status400BadRequest, (Enum?)null,
                    $"'{resource.Status}' is not a valid status. Expected 'ACKNOWLEDGED' or 'DISMISSED'.");
        }

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this, result, _problemDetailsFactory,
            alert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }
}