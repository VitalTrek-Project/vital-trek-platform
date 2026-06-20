using System.Net.Mime;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
// Corrected using directive
// For ProblemDetailsFactory

// For MonitoringError enum

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Incident endpoints")]
public class IncidentsController(
    IIncidentQueryService incidentQueryService,
    IIncidentCommandService incidentCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer, // Renamed for clarity
    ProblemDetailsFactory problemDetailsFactory) // Inject ProblemDetailsFactory
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet("{incidentId:int}")]
    [SwaggerOperation(
        Summary = "Get a incident by its id",
        Description = "Get a incident by its id",
        OperationId = "GetIncidentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incident was found", typeof(IncidentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The incident was not found")]
    public async Task<IActionResult> GetIncidentById([FromRoute] int incidentId, CancellationToken cancellationToken)
    {
        var getIncidentByIdQuery = new GetIncidentByIdQuery(incidentId);
        var incident = await incidentQueryService.Handle(getIncidentByIdQuery, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromGetIncidentByIdResult(
            this,
            incident,
            _errorLocalizer,
            _problemDetailsFactory,
            foundIncident => Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(foundIncident))
        );
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a incident",
        Description = "Create a incident",
        OperationId = "CreateIncident")]
    [SwaggerResponse(StatusCodes.Status201Created, "The incident was created", typeof(IncidentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The incident was not created")]
    public async Task<IActionResult> CreateIncident([FromBody] CreateIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var createIncidentCommand = CreateIncidentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await incidentCommandService.Handle(createIncidentCommand, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromCreateIncidentResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdIncident => CreatedAtAction(nameof(GetIncidentById), new { incidentId = createdIncident.Id },
                IncidentResourceFromEntityAssembler.ToResourceFromEntity(createdIncident))
        );
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all incidents",
        Description = "Get all incidents",
        OperationId = "GetAllIncidents")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incidents were found", typeof(IEnumerable<IncidentResource>))]
    public async Task<IActionResult> GetAllIncidents(CancellationToken cancellationToken)
    {
        var getAllIncidentsQuery = new GetAllIncidentsQuery();
        var incidents = await incidentQueryService.Handle(getAllIncidentsQuery, cancellationToken);
        var incidentResources = incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(incidentResources);
    }

}