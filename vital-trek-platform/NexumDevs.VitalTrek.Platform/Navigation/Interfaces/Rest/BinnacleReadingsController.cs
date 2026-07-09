using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest.Resources;
using Swashbuckle.AspNetCore.Annotations;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Binnacle Reading endpoints")]
public class BinnacleReadingsController(
    IBinnacleReadingCommandService binnacleReadingCommandService,
    IBinnacleReadingQueryService binnacleReadingQueryService)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Record a binnacle reading",
        Description = "Record a new binnacle reading for an expedition",
        OperationId = "RecordBinnacleReading")]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        "The binnacle reading was created",
        typeof(BinnacleReadingResource))]
    public async Task<IActionResult> RecordBinnacleReading(
        [FromBody] RecordBinnacleReadingCommand command,
        CancellationToken cancellationToken)
    {
        var reading =
            await binnacleReadingCommandService.Handle(
                command,
                cancellationToken);

        var resource =
            BinnacleReadingResourceFromEntityAssembler
                .ToResourceFromEntity(reading);

        return CreatedAtAction(
            nameof(GetBinnacleReadingsByExpedition),
            new { expeditionId = resource.ExpeditionId },
            resource);
    }

    [HttpGet("/api/v1/expeditions/{expeditionId:int}/binnacle-readings")]
    [SwaggerOperation(
        Summary = "Get binnacle readings by expedition",
        Description = "Get all binnacle readings for a specific expedition",
        OperationId = "GetBinnacleReadingsByExpedition")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "The binnacle readings were found",
        typeof(IEnumerable<BinnacleReadingResource>))]
    public async Task<IActionResult> GetBinnacleReadingsByExpedition(
        [FromRoute] int expeditionId,
        CancellationToken cancellationToken)
    {
        var query =
            new GetBinnacleReadingsByExpeditionQuery(expeditionId);

        var readings =
            await binnacleReadingQueryService.Handle(
                query,
                cancellationToken);

        var resources =
            readings.Select(
                BinnacleReadingResourceFromEntityAssembler
                    .ToResourceFromEntity);

        return Ok(resources);
    }
}
