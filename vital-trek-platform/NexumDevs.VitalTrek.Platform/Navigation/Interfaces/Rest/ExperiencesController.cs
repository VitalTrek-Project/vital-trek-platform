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
[SwaggerTag("Available Expeditions endpoints")]
public class ExperiencesController(
    IExperienceQueryService experienceQueryService,
    IExperienceCommandService experienceCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    
    [HttpGet("{experienceId:int}")]
    [SwaggerOperation(
        Summary = "Get an experience by its id",
        Description = "Get an experience by its id",
        OperationId = "GetExperienceById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The experience was found", typeof(ExperienceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The experience was not found")]
    public async Task<IActionResult> GetExperienceById([FromRoute] int experienceId, CancellationToken cancellationToken)
    {
        var getExperienceByIdQuery = new GetExperienceByIdQuery(experienceId);
        var experience = await experienceQueryService.Handle(getExperienceByIdQuery, cancellationToken);

        return NavigationActionResultAssembler.ToActionResultFromGetExperienceByIdResult(
            this,
            experience,
            _errorLocalizer,
            _problemDetailsFactory,
            foundExperience => Ok(ExperienceResourceFromEntityAssembler.ToResourceFromEntity(foundExperience))
        );
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create an experience",
        Description = "Create an experience",
        OperationId = "CreateExperience")]
    [SwaggerResponse(StatusCodes.Status201Created, "The experience was created", typeof(ExperienceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The experience was not created")]
    public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceResource resource,
        CancellationToken cancellationToken)
    {
        var createExperienceCommand = CreateExperienceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await experienceCommandService.Handle(createExperienceCommand, cancellationToken);
        
        return NavigationActionResultAssembler.ToActionResultFromCreateExperienceResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdExperience => CreatedAtAction(nameof(GetExperienceById), new { experienceId = createdExperience.Id }, 
                ExperienceResourceFromEntityAssembler.ToResourceFromEntity(createdExperience))
        );
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all experience",
        Description = "Get all experience",
        OperationId = "GetAllExperiences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The experiences were found", typeof(IEnumerable<ExperienceResource>))]
    public async Task<IActionResult> GetAllExperiences(CancellationToken cancellationToken)
    {
        var getAllExperiencesQuery = new GetAllExperiencesQuery();
        var experiences = await experienceQueryService.Handle(getAllExperiencesQuery, cancellationToken);
        var experiencesResources = experiences.Select(ExperienceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(experiencesResources);
    }
}
