using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Transform;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest;

/// <summary>
/// REST controller responsible for managing tourist assignments within tours.
/// Provides endpoints for assigning tourists, removing assignments,
/// and retrieving assignments associated with a specific tour.
/// </summary>
[ApiController]
[Route("api/v1/tours/{tourId:guid}/assignments")]
public class TourAssignmentsController : ControllerBase
{
    private readonly ITourCommandService _tourCommandService;
    private readonly ITourQueryService _tourQueryService;
    private readonly IStringLocalizer _errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TourAssignmentsController"/> class.
    /// </summary>
    /// <param name="tourCommandService">
    /// Service responsible for handling tour-related commands.
    /// </param>
    /// <param name="tourQueryService">
    /// Service responsible for handling tour-related queries.
    /// </param>
    /// <param name="errorLocalizer">
    /// Localizer used to retrieve translated error messages.
    /// </param>
    /// <param name="problemDetailsFactory">
    /// Factory used to create standardized problem details responses.
    /// </param>
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

    /// <summary>
    /// Assigns a tourist to the specified tour.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="resource">
    /// The resource containing the tourist information.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the created assignment resource.
    /// </returns>
    /// <response code="200">
    /// The tourist was successfully assigned to the tour.
    /// </response>
    /// <response code="400">
    /// The assignment request is invalid.
    /// </response>
    [HttpPost]
    public async Task<IActionResult> AssignTourist(
        Guid tourId,
        [FromBody] AssignTouristResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new AssignTouristCommand(tourId, resource.TouristId);
            var assignment = await _tourCommandService.Handle(command, cancellationToken);

            return Ok(TourAssignmentResourceFromEntityAssembler.ToResourceFromEntity(assignment));
        }
        catch (TourManagementError error)
        {
            // Unlike the other actions in this controller (which only ever throw one error
            // kind), AssignTourist can fail with TourNotFound as well as several validation
            // errors — each needs its own status code, not a single hardcoded 400.
            var statusCode = error.ErrorCode == TourManagementErrors.TourNotFound
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;

            return StatusCode(statusCode, _problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    /// <summary>
    /// Removes a tourist assignment from the specified tour.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="touristId">
    /// The unique identifier of the tourist to unassign.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="NoContentResult"/> when the assignment is successfully removed.
    /// </returns>
    /// <response code="204">
    /// The tourist was successfully unassigned from the tour.
    /// </response>
    /// <response code="404">
    /// The tour or assignment was not found.
    /// </response>
    [HttpDelete("{touristId:guid}")]
    public async Task<IActionResult> UnassignTourist(
        Guid tourId,
        Guid touristId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tourCommandService.Handle(
                new UnassignTouristCommand(tourId, touristId),
                cancellationToken);

            return NoContent();
        }
        catch (TourManagementError error)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                StatusCodes.Status404NotFound,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    /// <summary>
    /// Retrieves all tourist assignments associated with the specified tour.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="TourAssignmentResource"/> objects.
    /// </returns>
    /// <response code="200">
    /// The assignments were successfully retrieved.
    /// </response>
    [HttpGet]
    public async Task<IActionResult> GetAssignedTourists(
        Guid tourId,
        CancellationToken cancellationToken)
    {
        var assignments = await _tourQueryService.Handle(
            new GetAssignedTouristsByTourQuery(tourId),
            cancellationToken);

        var resources = assignments.Select(
            TourAssignmentResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }
}