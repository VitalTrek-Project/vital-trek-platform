using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Application.QueryServices;

public interface IDashboardQueryService
{
    Task<AdminDashboardSummary> Handle(GetAdminDashboardSummaryQuery query, CancellationToken cancellationToken);

    Task<AlertsDistribution> Handle(GetAlertsDistributionQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpeditionTimeSeriesPoint>> Handle(GetExpeditionsTimeSeriesQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<Alert>> Handle(GetAlertsRequiringAttentionQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpeditionActivitySummary>> Handle(GetActiveExpeditionsQuery query, CancellationToken cancellationToken);

    Task<TouristDashboard> Handle(GetTouristDashboardQuery query, CancellationToken cancellationToken);
}
