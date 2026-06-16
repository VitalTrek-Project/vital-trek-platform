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
[SwaggerTag("Available Weather endpoints")]
public class WeatherController(
    IWeatherQueryService weatherQueryService,
    IWeatherCommandService weatherCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    
    [HttpGet("{weatherId:int}")]
    [SwaggerOperation(
        Summary = "Get a weather by its id",
        Description = "Get a weather by its id",
        OperationId = "GetWeatherById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The weather was found", typeof(WeatherResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The weather was not found")]
    public async Task<IActionResult> GetWeatherById([FromRoute] int weatherId, CancellationToken cancellationToken)
    {
        var getWeatherByIdQuery = new GetWeatherByIdQuery(weatherId);
        var weather = await weatherQueryService.Handle(getWeatherByIdQuery, cancellationToken);

        return NavigationActionResultAssembler.ToActionResultFromGetWeatherByIdResult(
            this,
            weather,
            _errorLocalizer,
            _problemDetailsFactory,
            foundWeather => Ok(WeatherResourceFromEntityAssembler.ToResourceFromEntity(foundWeather))
        );
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a weather",
        Description = "Create a weather",
        OperationId = "CreateWeather")]
    [SwaggerResponse(StatusCodes.Status201Created, "The weather was created", typeof(WeatherResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The weather was not created")]
    public async Task<IActionResult> CreateWeather([FromBody] CreateWeatherResource resource,
        CancellationToken cancellationToken)
    {
        var createWeatherCommand = CreateWeatherCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await weatherCommandService.Handle(createWeatherCommand, cancellationToken);
        
        return NavigationActionResultAssembler.ToActionResultFromCreateWeatherResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdWeather => CreatedAtAction(nameof(GetWeatherById), new { weatherId = createdWeather.Id }, 
                WeatherResourceFromEntityAssembler.ToResourceFromEntity(createdWeather))
        );
    }
}
