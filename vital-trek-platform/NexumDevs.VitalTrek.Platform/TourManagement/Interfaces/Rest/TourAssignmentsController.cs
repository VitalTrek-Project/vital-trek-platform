using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Transform;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest;

/// <summary>
/// Controller "anidado" bajo /tours/{tourId}/assignments.
/// Equivalente a "CategoryTutorialsController" en Publishing
/// (recurso hijo de otro recurso).
/// </summary>
[ApiController]
[Route("api/v1/tours/{tourId:guid}/assignments")]
public class TourAssignmentsController : ControllerBase
{
    private readonly ITourCommandService _tourCommandService;
    private readonly ITourQueryService _tourQueryService;
    private readonly IStringLocalizer _errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public TourAssignmentsController(
        ITourCommandService tourCommandService,
        ITourQueryService tourQueryService,
        IStringLocalizer errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _tourCommandService = tourCommandService;
        _tourQueryService = tourQueryService;
        _errorLocalizer = errorLocalizer;
        _problemDetailsFactory = problemDetailsFactory;
    }

    [HttpPost]
    public async Task<IActionResult> AssignTourist(Guid tourId, [FromBody] AssignTouristResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new AssignTouristCommand(tourId, resource.TouristId);
            var assignment = await _tourCommandService.Handle(command, cancellationToken);

            return Ok(TourAssignmentResourceFromEntityAssembler.ToResourceFromEntity(assignment));
        }
        catch (TourManagementError error)
        {
            return BadRequest(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    [HttpDelete("{touristId:guid}")]
    public async Task<IActionResult> UnassignTourist(Guid tourId, Guid touristId, CancellationToken cancellationToken)
    {
        try
        {
            await _tourCommandService.Handle(new UnassignTouristCommand(tourId, touristId), cancellationToken);
            return NoContent();
        }
        catch (TourManagementError error)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignedTourists(Guid tourId, CancellationToken cancellationToken)
    {
        var assignments = await _tourQueryService.Handle(new GetAssignedTouristsByTourQuery(tourId), cancellationToken);
        var resources = assignments.Select(TourAssignmentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}