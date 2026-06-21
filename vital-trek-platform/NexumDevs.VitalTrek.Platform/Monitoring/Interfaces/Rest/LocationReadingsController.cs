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
[SwaggerTag("Available Location Reading endpoints")]
public class LocationReadingsController(
    ILocationReadingCommandService locationReadingCommandService,
    ILocationReadingQueryService locationReadingQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Record a location reading",
        Description = "Record a new location reading for a tourist in an expedition",
        OperationId = "RecordLocationReading")]
    [SwaggerResponse(StatusCodes.Status201Created, "The location reading was created", typeof(LocationReadingResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The location reading was not created")]
    public async Task<IActionResult> RecordLocationReading([FromBody] RecordLocationCommand command, CancellationToken cancellationToken)
    {
        var result = await locationReadingCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, result.Error, result.Message);
        }
        var resource = LocationReadingResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GetLocationReadingsByExpedition), new { expeditionId = resource.ExpeditionId }, resource);
    }

    [HttpGet("expedition/{expeditionId:int}")]
    [SwaggerOperation(
        Summary = "Get location readings by expedition",
        Description = "Get all location readings for a specific expedition",
        OperationId = "GetLocationReadingsByExpedition")]
    [SwaggerResponse(StatusCodes.Status200OK, "The location readings were found", typeof(IEnumerable<LocationReadingResource>))]
    public async Task<IActionResult> GetLocationReadingsByExpedition([FromRoute] int expeditionId, CancellationToken cancellationToken)
    {
        var query = new GetLocationReadingsByExpeditionQuery(expeditionId);
        var readings = await locationReadingQueryService.Handle(query, cancellationToken);
        var resources = readings.Select(LocationReadingResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}