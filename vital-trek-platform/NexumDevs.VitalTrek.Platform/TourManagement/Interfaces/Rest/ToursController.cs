using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest.Transform;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Rest;

/// <summary>
/// REST controller responsible for managing tours.
/// Provides endpoints for creating, retrieving, updating,
/// deleting, searching, and duplicating tours.
/// </summary>
[ApiController]
[Route("api/v1/tours")]
public class ToursController : ControllerBase
{
    private readonly ITourCommandService _tourCommandService;
    private readonly ITourQueryService _tourQueryService;
    private readonly IStringLocalizer _errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToursController"/> class.
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
    public ToursController(
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
    /// Creates a new tour.
    /// </summary>
    /// <param name="resource">
    /// The resource containing the information required to create the tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="CreatedAtActionResult"/> containing the created tour resource.
    /// </returns>
    /// <response code="201">
    /// The tour was successfully created.
    /// </response>
    /// <response code="400">
    /// The request contains invalid data.
    /// </response>
    [HttpPost]
    public async Task<IActionResult> CreateTour(
        [FromBody] CreateTourResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = TourCommandFromResourceAssembler.ToCreateCommandFromResource(resource);
            var tour = await _tourCommandService.Handle(command, cancellationToken);

            var resourceResult = TourResourceFromEntityAssembler.ToResourceFromEntity(tour);

            return CreatedAtAction(
                nameof(GetTourById),
                new { tourId = resourceResult.Id },
                resourceResult);
        }
        catch (TourManagementError error)
        {
            return BadRequest(_problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                StatusCodes.Status400BadRequest,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    /// <summary>
    /// Retrieves a tour by its unique identifier.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The requested <see cref="TourResource"/> if found.
    /// </returns>
    /// <response code="200">
    /// The tour was successfully retrieved.
    /// </response>
    /// <response code="404">
    /// The specified tour was not found.
    /// </response>
    [HttpGet("{tourId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTourById(
        Guid tourId,
        CancellationToken cancellationToken)
    {
        var tour = await _tourQueryService.Handle(
            new GetTourByIdQuery(tourId),
            cancellationToken);

        if (tour is null)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                StatusCodes.Status404NotFound,
                detail: _errorLocalizer[TourManagementErrors.TourNotFound]));
        }

        return Ok(TourResourceFromEntityAssembler.ToResourceFromEntity(tour));
    }

    /// <summary>
    /// Retrieves tours from the collection, optionally filtered by agency or a search term.
    /// Replaces the old separate <c>agency/{agencyId}</c> and <c>search</c> action-like routes
    /// with query parameters on the single collection URI, per REST resource-naming conventions.
    /// </summary>
    /// <param name="agencyId">
    /// When provided, restricts the results to tours belonging to this agency.
    /// </param>
    /// <param name="term">
    /// When provided (and <paramref name="agencyId"/> is not), restricts the results to tours
    /// whose title or description contain this term. Omitting both returns every tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="TourResource"/> objects.
    /// </returns>
    /// <response code="200">
    /// The tours were successfully retrieved.
    /// </response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTours(
        [FromQuery] Guid? agencyId,
        [FromQuery] string? term,
        CancellationToken cancellationToken)
    {
        var tours = agencyId is { } id
            ? await _tourQueryService.Handle(new GetAllToursByAgencyQuery(id), cancellationToken)
            : await _tourQueryService.Handle(new SearchToursQuery(term ?? string.Empty), cancellationToken);

        var resources = tours.Select(
            TourResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    /// <summary>
    /// Updates an existing tour.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour to update.
    /// </param>
    /// <param name="resource">
    /// The resource containing the updated tour information.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The updated <see cref="TourResource"/>.
    /// </returns>
    /// <response code="200">
    /// The tour was successfully updated.
    /// </response>
    /// <response code="404">
    /// The specified tour was not found.
    /// </response>
    [HttpPut("{tourId:guid}")]
    public async Task<IActionResult> UpdateTour(
        Guid tourId,
        [FromBody] UpdateTourResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = TourCommandFromResourceAssembler
                .ToUpdateCommandFromResource(resource, tourId);

            var tour = await _tourCommandService.Handle(command, cancellationToken);

            return Ok(TourResourceFromEntityAssembler.ToResourceFromEntity(tour));
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
    /// Deletes a tour.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour to delete.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="NoContentResult"/> when the tour is successfully deleted.
    /// </returns>
    /// <response code="204">
    /// The tour was successfully deleted.
    /// </response>
    /// <response code="404">
    /// The specified tour was not found.
    /// </response>
    [HttpDelete("{tourId:guid}")]
    public async Task<IActionResult> DeleteTour(
        Guid tourId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tourCommandService.Handle(
                new DeleteTourCommand(tourId),
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
    /// Creates a copy of an existing tour. Modeled as POSTing a new element into the tour's
    /// "copies" sub-collection (a noun) rather than a verb-suffixed <c>/duplicate</c> action.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour to duplicate.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="CreatedAtActionResult"/> containing the duplicated tour.
    /// </returns>
    /// <response code="201">
    /// The tour was successfully duplicated.
    /// </response>
    /// <response code="404">
    /// The specified tour was not found.
    /// </response>
    [HttpPost("{tourId:guid}/copies")]
    public async Task<IActionResult> DuplicateTour(
        Guid tourId,
        CancellationToken cancellationToken)
    {
        try
        {
            var duplicated = await _tourCommandService.Handle(
                new DuplicateTourCommand(tourId),
                cancellationToken);

            var resourceResult =
                TourResourceFromEntityAssembler.ToResourceFromEntity(duplicated);

            return CreatedAtAction(
                nameof(GetTourById),
                new { tourId = resourceResult.Id },
                resourceResult);
        }
        catch (TourManagementError error)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                StatusCodes.Status404NotFound,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }
}