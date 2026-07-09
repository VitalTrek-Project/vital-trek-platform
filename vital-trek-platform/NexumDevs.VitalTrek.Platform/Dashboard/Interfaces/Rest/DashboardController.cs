using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumDevs.VitalTrek.Platform.Dashboard.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest;

/// <summary>
/// Read-only analytics endpoints for the agency-admin dashboard and the tourist dashboard.
/// Resources are nested hierarchically under nouns (e.g. alerts/distribution,
/// expeditions/active) — no "admin" path segment, since that describes an audience, not a
/// resource; the [Authorize(Roles=Agency)] attribute already scopes access.
/// These endpoints are not yet scoped per-agency (there is no AgencyId on the underlying
/// Alert/Expedition data), so they are restricted to the Agency role as a whole rather than to a
/// single agency's data — narrowing that further requires adding AgencyId to those aggregates.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Dashboard endpoints")]
public class DashboardController(IDashboardQueryService dashboardQueryService, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [Authorize(Roles = nameof(UserRole.Agency))]
    [HttpGet("summary")]
    [SwaggerOperation(
        Summary = "Get admin dashboard KPI summary",
        Description = "KPIs for expeditions, alerts, active tourists and staff, each compared against the previous period of equal length",
        OperationId = "GetAdminDashboardSummary")]
    [SwaggerResponse(StatusCodes.Status200OK, "The summary was computed", typeof(AdminDashboardSummaryResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "'from' is after 'to'")]
    public async Task<IActionResult> GetAdminSummary(
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        CancellationToken cancellationToken)
    {
        var (resolvedFrom, resolvedTo) = ResolveRange(from, to);
        if (resolvedFrom > resolvedTo) return InvalidRange();

        var summary = await dashboardQueryService.Handle(new GetAdminDashboardSummaryQuery(resolvedFrom, resolvedTo), cancellationToken);
        return Ok(DashboardResourceAssembler.ToResourceFromSummary(summary));
    }

    [Authorize(Roles = nameof(UserRole.Agency))]
    [HttpGet("alerts/distribution")]
    [SwaggerOperation(
        Summary = "Get alerts distribution",
        Description = "Alert counts grouped by severity and by type within the given period",
        OperationId = "GetAdminAlertsDistribution")]
    [SwaggerResponse(StatusCodes.Status200OK, "The distribution was computed", typeof(AlertsDistributionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "'from' is after 'to'")]
    public async Task<IActionResult> GetAlertsDistribution(
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        CancellationToken cancellationToken)
    {
        var (resolvedFrom, resolvedTo) = ResolveRange(from, to);
        if (resolvedFrom > resolvedTo) return InvalidRange();

        var distribution = await dashboardQueryService.Handle(new GetAlertsDistributionQuery(resolvedFrom, resolvedTo), cancellationToken);
        return Ok(DashboardResourceAssembler.ToResourceFromDistribution(distribution));
    }

    [Authorize(Roles = nameof(UserRole.Agency))]
    [HttpGet("alerts/pending")]
    [SwaggerOperation(
        Summary = "Get alerts requiring attention",
        Description = "Active (unacknowledged) alerts ordered by severity then age",
        OperationId = "GetAdminAlertsRequiringAttention")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alerts were found", typeof(IEnumerable<AttentionAlertResource>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "'take' is out of range")]
    public async Task<IActionResult> GetAlertsRequiringAttention(
        [FromQuery, Range(1, 100, ErrorMessage = "'take' must be between 1 and 100.")] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var alerts = await dashboardQueryService.Handle(new GetAlertsRequiringAttentionQuery(take), cancellationToken);
        return Ok(alerts.Select(DashboardResourceAssembler.ToAttentionResourceFromEntity));
    }

    [Authorize(Roles = nameof(UserRole.Agency))]
    [HttpGet("expeditions/timeseries")]
    [SwaggerOperation(
        Summary = "Get expeditions time series",
        Description = "Number of expeditions created per week or month within the given period",
        OperationId = "GetAdminExpeditionsTimeSeries")]
    [SwaggerResponse(StatusCodes.Status200OK, "The time series was computed", typeof(IEnumerable<ExpeditionsTimeSeriesPointResource>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "'from' is after 'to', or 'bucket' is not 'week'/'month'")]
    public async Task<IActionResult> GetExpeditionsTimeSeries(
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        [FromQuery, RegularExpression("^(week|month)$", ErrorMessage = "'bucket' must be 'week' or 'month'.")] string bucket = "week",
        CancellationToken cancellationToken = default)
    {
        var (resolvedFrom, resolvedTo) = ResolveRange(from, to);
        if (resolvedFrom > resolvedTo) return InvalidRange();

        var points = await dashboardQueryService.Handle(new GetExpeditionsTimeSeriesQuery(resolvedFrom, resolvedTo, bucket), cancellationToken);
        return Ok(points.Select(DashboardResourceAssembler.ToResourceFromTimeSeriesPoint));
    }

    [Authorize(Roles = nameof(UserRole.Agency))]
    [HttpGet("expeditions/active")]
    [SwaggerOperation(
        Summary = "Get expeditions in progress",
        Description = "Expeditions currently in progress, with their tourist count",
        OperationId = "GetAdminActiveExpeditions")]
    [SwaggerResponse(StatusCodes.Status200OK, "The expeditions were found", typeof(IEnumerable<ActiveExpeditionResource>))]
    public async Task<IActionResult> GetActiveExpeditions(CancellationToken cancellationToken)
    {
        var expeditions = await dashboardQueryService.Handle(new GetActiveExpeditionsQuery(), cancellationToken);
        return Ok(expeditions.Select(DashboardResourceAssembler.ToResourceFromActivitySummary));
    }

    // TODO: touristId is an int from Monitoring/Navigation's own tourist ID scheme, with no
    // bridge to the Iam Guid user id carried in the JWT — so unlike the admin/* endpoints above,
    // this can't verify the caller actually IS that tourist (same known gap as the loyalty
    // ProgramController/ProfileController). [Authorize] at least requires *some* valid session;
    // it does not yet stop one tourist from reading another tourist's dashboard by touristId.
    [Authorize]
    [HttpGet("tourists/{touristId:int}/summary")]
    [SwaggerOperation(
        Summary = "Get tourist dashboard",
        Description = "Current expedition (inferred from the tourist's most recent activity), personal alerts and past expeditions",
        OperationId = "GetTouristDashboard")]
    [SwaggerResponse(StatusCodes.Status200OK, "The dashboard was computed", typeof(TouristDashboardResource))]
    public async Task<IActionResult> GetTouristDashboard([FromRoute] int touristId, CancellationToken cancellationToken)
    {
        var dashboard = await dashboardQueryService.Handle(new GetTouristDashboardQuery(touristId), cancellationToken);
        return Ok(DashboardResourceAssembler.ToResourceFromTouristDashboard(dashboard));
    }

    private IActionResult InvalidRange() =>
        problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status400BadRequest, (Enum?)null, "'from' must not be after 'to'.");

    private static (DateTimeOffset From, DateTimeOffset To) ResolveRange(DateTimeOffset? from, DateTimeOffset? to)
    {
        var resolvedTo = to ?? DateTimeOffset.UtcNow;
        var resolvedFrom = from ?? resolvedTo.AddDays(-30);
        return (resolvedFrom, resolvedTo);
    }
}
