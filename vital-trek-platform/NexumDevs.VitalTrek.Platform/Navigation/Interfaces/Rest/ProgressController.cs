using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Progress endpoints")]
public class ProgressController(
    IProgressQueryService progressQueryService,
    IProgressCommandService progressCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    
    [HttpGet("{progressId:int}")]
    [SwaggerOperation(
        Summary = "Get a progress by its id",
        Description = "Get a progress by its id",
        OperationId = "GetProgressById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The progress was found", typeof(ProgressResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The progress was not found")]
    public async Task<IActionResult> GetProgressById([FromRoute] int progressId, CancellationToken cancellationToken)
    {
        var getProgressByIdQuery = new GetProgressByIdQuery(progressId);
        var progress = await progressQueryService.Handle(getProgressByIdQuery, cancellationToken);

        return NavigationActionResultAssembler.ToActionResultFromGetProgressByIdResult(
            this,
            progress,
            _errorLocalizer,
            _problemDetailsFactory,
            foundProgress => Ok(ProgressResourceFromEntityAssembler.ToResourceFromEntity(foundProgress))
        );
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a progress",
        Description = "Create a progress",
        OperationId = "CreateProgress")]
    [SwaggerResponse(StatusCodes.Status201Created, "The progress was created", typeof(ProgressResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The progress was not created")]
    public async Task<IActionResult> CreateProgress([FromBody] CreateProgressResource resource,
        CancellationToken cancellationToken)
    {
        var createProgressCommand = CreateProgressCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await progressCommandService.Handle(createProgressCommand, cancellationToken);
        
        return NavigationActionResultAssembler.ToActionResultFromCreateProgressResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdProgress => CreatedAtAction(nameof(GetProgressById), new { progressId = createdProgress.Id }, 
                ProgressResourceFromEntityAssembler.ToResourceFromEntity(createdProgress))
        );
    }
}
