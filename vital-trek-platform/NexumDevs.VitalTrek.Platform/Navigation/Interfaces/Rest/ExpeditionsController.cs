using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
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
public class ExpeditionsController(
    IExpeditionQueryService expeditionQueryService,
    IExpeditionCommandService expeditionCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    
    [HttpGet("{expeditionId:int}")]
    [SwaggerOperation(
        Summary = "Get an expedition by its id",
        Description = "Get an expedition by its id",
        OperationId = "GetExpeditionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The expedition was found", typeof(ExpeditionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The expedition was not found")]
    public async Task<IActionResult> GetExpeditionById([FromRoute] int expeditionId, CancellationToken cancellationToken)
    {
        var getExpeditionByIdQuery = new GetExpeditionByIdQuery(expeditionId);
        var expedition = await expeditionQueryService.Handle(getExpeditionByIdQuery, cancellationToken);

        return NavigationActionResultAssembler.ToActionResultFromGetExpeditionByIdResult(
            this,
            expedition,
            _errorLocalizer,
            _problemDetailsFactory,
            foundExpedition => Ok(ExpeditionResourceFromEntityAssembler.ToResourceFromEntity(foundExpedition))
        );
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a expedition",
        Description = "Create a expedition",
        OperationId = "CreateExpedition")]
    [SwaggerResponse(StatusCodes.Status201Created, "The expedition was created", typeof(ExpeditionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The expedition was not created")]
    public async Task<IActionResult> CreateExpedition([FromBody] CreateExpeditionResource resource,
        CancellationToken cancellationToken)
    {
        var createExpeditionCommand = CreateExpeditionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await expeditionCommandService.Handle(createExpeditionCommand, cancellationToken);
        
        return NavigationActionResultAssembler.ToActionResultFromCreateExpeditionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdExpedition => CreatedAtAction(nameof(GetExpeditionById), new { expeditionId = createdExpedition.Id }, 
                ExpeditionResourceFromEntityAssembler.ToResourceFromEntity(createdExpedition))
        );
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all expeditions",
        Description = "Get all expeditions",
        OperationId = "GetAllExpeditions")]
    [SwaggerResponse(StatusCodes.Status200OK, "The expeditions were found", typeof(IEnumerable<ExpeditionResource>))]
    public async Task<IActionResult> GetAllExpeditions(CancellationToken cancellationToken)
    {
        var getAllExpeditionsQuery = new GetAllExpeditionsQuery();
        var expeditions = await expeditionQueryService.Handle(getAllExpeditionsQuery, cancellationToken);
        var expeditionsResources = expeditions.Select(ExpeditionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(expeditionsResources);
    }
}
