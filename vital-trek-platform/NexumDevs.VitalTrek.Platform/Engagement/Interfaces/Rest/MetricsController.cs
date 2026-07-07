using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>Aggregated loyalty metrics backing the admin dashboard's loyalty cards.</summary>
[ApiController]
[Route("api/v1/loyalty")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Loyalty metrics endpoints")]
public class MetricsController(IMetricsQueryService queryService) : ControllerBase
{
    [HttpGet("agencies/{agencyId:guid}/metrics")]
    [SwaggerOperation(
        Summary = "Get loyalty metrics for an agency",
        Description = "Points issued vs. redeemed, tourists per tier, and the top tourists by balance.",
        OperationId = "GetLoyaltyMetrics")]
    [SwaggerResponse(StatusCodes.Status200OK, "The metrics were computed", typeof(LoyaltyMetricsResource))]
    public async Task<IActionResult> GetMetrics([FromRoute] Guid agencyId, CancellationToken cancellationToken)
    {
        var metrics = await queryService.Handle(new GetLoyaltyMetricsQuery(agencyId), cancellationToken);
        return Ok(MetricsResourceAssembler.ToResourceFromMetrics(metrics));
    }
}
