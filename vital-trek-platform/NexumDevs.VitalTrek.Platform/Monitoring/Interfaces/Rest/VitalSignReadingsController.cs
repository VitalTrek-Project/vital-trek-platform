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
[SwaggerTag("Available Vital Sign Reading endpoints")]
public class VitalSignReadingsController(
    IVitalSignReadingCommandService vitalSignReadingCommandService,
    IVitalSignReadingQueryService vitalSignReadingQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Record a vital sign reading",
        Description = "Record a new vital sign reading for a tourist in an expedition",
        OperationId = "RecordVitalSignReading")]
    [SwaggerResponse(StatusCodes.Status201Created, "The vital sign reading was created", typeof(VitalSignReadingResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The vital sign reading was not created")]
    public async Task<IActionResult> RecordVitalSignReading([FromBody] RecordVitalSignsCommand command, CancellationToken cancellationToken)
    {
        var result = await vitalSignReadingCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this, result, _problemDetailsFactory,
            reading =>
            {
                var resource = VitalSignReadingResourceFromEntityAssembler.ToResourceFromEntity(reading);
                return CreatedAtAction(nameof(GetVitalSignReadingsByExpedition), new { expeditionId = resource.ExpeditionId }, resource);
            });
    }

    [HttpGet("/api/v1/expeditions/{expeditionId:int}/vital-sign-readings")]
    [SwaggerOperation(
        Summary = "Get vital sign readings by expedition",
        Description = "Get all vital sign readings for a specific expedition",
        OperationId = "GetVitalSignReadingsByExpedition")]
    [SwaggerResponse(StatusCodes.Status200OK, "The vital sign readings were found", typeof(IEnumerable<VitalSignReadingResource>))]
    public async Task<IActionResult> GetVitalSignReadingsByExpedition([FromRoute] int expeditionId, CancellationToken cancellationToken)
    {
        var query = new GetVitalSignReadingsByExpeditionQuery(expeditionId);
        var readings = await vitalSignReadingQueryService.Handle(query, cancellationToken);
        var resources = readings.Select(VitalSignReadingResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}