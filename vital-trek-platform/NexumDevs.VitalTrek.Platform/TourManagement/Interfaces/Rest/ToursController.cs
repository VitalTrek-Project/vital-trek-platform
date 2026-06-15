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
/// Equivalente a "CategoriesController" / "TutorialsController" en Publishing.
/// </summary>
[ApiController]
[Route("api/v1/tours")]
public class ToursController : ControllerBase
{
    private readonly ITourCommandService _tourCommandService;
    private readonly ITourQueryService _tourQueryService;
    private readonly IStringLocalizer _errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

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

    [HttpPost]
    public async Task<IActionResult> CreateTour([FromBody] CreateTourResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = TourCommandFromResourceAssembler.ToCreateCommandFromResource(resource);
            var tour = await _tourCommandService.Handle(command, cancellationToken);

            var resourceResult = TourResourceFromEntityAssembler.ToResourceFromEntity(tour);
            return CreatedAtAction(nameof(GetTourById), new { tourId = resourceResult.Id }, resourceResult);
        }
        catch (TourManagementError error)
        {
            return BadRequest(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    [HttpGet("{tourId:guid}")]
    public async Task<IActionResult> GetTourById(Guid tourId, CancellationToken cancellationToken)
    {
        var tour = await _tourQueryService.Handle(new GetTourByIdQuery(tourId), cancellationToken);

        if (tour is null)
            return NotFound(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: _errorLocalizer[TourManagementErrors.TourNotFound]));

        return Ok(TourResourceFromEntityAssembler.ToResourceFromEntity(tour));
    }

    [HttpGet("agency/{agencyId:guid}")]
    public async Task<IActionResult> GetAllToursByAgency(Guid agencyId, CancellationToken cancellationToken)
    {
        var tours = await _tourQueryService.Handle(new GetAllToursByAgencyQuery(agencyId), cancellationToken);
        var resources = tours.Select(TourResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTours([FromQuery] string term, CancellationToken cancellationToken)
    {
        var tours = await _tourQueryService.Handle(new SearchToursQuery(term), cancellationToken);
        var resources = tours.Select(TourResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPut("{tourId:guid}")]
    public async Task<IActionResult> UpdateTour(Guid tourId, [FromBody] UpdateTourResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = TourCommandFromResourceAssembler.ToUpdateCommandFromResource(resource, tourId);
            var tour = await _tourCommandService.Handle(command, cancellationToken);

            return Ok(TourResourceFromEntityAssembler.ToResourceFromEntity(tour));
        }
        catch (TourManagementError error)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    [HttpDelete("{tourId:guid}")]
    public async Task<IActionResult> DeleteTour(Guid tourId, CancellationToken cancellationToken)
    {
        try
        {
            await _tourCommandService.Handle(new DeleteTourCommand(tourId), cancellationToken);
            return NoContent();
        }
        catch (TourManagementError error)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }

    [HttpPost("{tourId:guid}/duplicate")]
    public async Task<IActionResult> DuplicateTour(Guid tourId, CancellationToken cancellationToken)
    {
        try
        {
            var duplicated = await _tourCommandService.Handle(new DuplicateTourCommand(tourId), cancellationToken);

            var resourceResult = TourResourceFromEntityAssembler.ToResourceFromEntity(duplicated);
            return CreatedAtAction(nameof(GetTourById), new { tourId = resourceResult.Id }, resourceResult);
        }
        catch (TourManagementError error)
        {
            return NotFound(_problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: _errorLocalizer[error.ErrorCode]));
        }
    }
}