using NexumDevs.VitalTrek.Platform.Dashboard.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Application.Internal.QueryServices;

public class DashboardQueryService(IDashboardRepository dashboardRepository) : IDashboardQueryService
{
    public async Task<AdminDashboardSummary> Handle(GetAdminDashboardSummaryQuery query, CancellationToken cancellationToken)
    {
        var duration = query.To - query.From;
        var previousFrom = query.From - duration;
        var previousTo = query.From;

        var expeditionsActive = await dashboardRepository.CountExpeditionsByStatusKeyAsync("active", query.From, query.To, cancellationToken);
        var expeditionsActivePrevious = await dashboardRepository.CountExpeditionsByStatusKeyAsync("active", previousFrom, previousTo, cancellationToken);

        var expeditionsCompleted = await dashboardRepository.CountExpeditionsByStatusKeyAsync("finished", query.From, query.To, cancellationToken);
        var expeditionsCompletedPrevious = await dashboardRepository.CountExpeditionsByStatusKeyAsync("finished", previousFrom, previousTo, cancellationToken);

        var alertsOpen = await dashboardRepository.CountActiveAlertsAsync(query.From, query.To, cancellationToken);
        var alertsOpenPrevious = await dashboardRepository.CountActiveAlertsAsync(previousFrom, previousTo, cancellationToken);
        var alertsOpenBySeverity = await dashboardRepository.CountAlertsBySeverityAsync(query.From, query.To, cancellationToken);

        var touristsActive = await dashboardRepository.CountDistinctActiveTouristsAsync(query.From, query.To, cancellationToken);
        var touristsActivePrevious = await dashboardRepository.CountDistinctActiveTouristsAsync(previousFrom, previousTo, cancellationToken);

        var staffAssigned = await dashboardRepository.CountDistinctGuidesAsync(query.From, query.To, cancellationToken);
        var staffAssignedPrevious = await dashboardRepository.CountDistinctGuidesAsync(previousFrom, previousTo, cancellationToken);

        return new AdminDashboardSummary(
            new KpiValue(expeditionsActive, expeditionsActivePrevious),
            new KpiValue(expeditionsCompleted, expeditionsCompletedPrevious),
            new KpiValue(alertsOpen, alertsOpenPrevious),
            alertsOpenBySeverity,
            new KpiValue(touristsActive, touristsActivePrevious),
            new KpiValue(staffAssigned, staffAssignedPrevious));
    }

    public async Task<AlertsDistribution> Handle(GetAlertsDistributionQuery query, CancellationToken cancellationToken)
    {
        var bySeverity = await dashboardRepository.CountAlertsBySeverityAsync(query.From, query.To, cancellationToken);
        var byType = await dashboardRepository.CountAlertsByTypeAsync(query.From, query.To, cancellationToken);
        return new AlertsDistribution(bySeverity, byType);
    }

    public async Task<IReadOnlyList<ExpeditionTimeSeriesPoint>> Handle(GetExpeditionsTimeSeriesQuery query, CancellationToken cancellationToken)
    {
        return await dashboardRepository.GetExpeditionsTimeSeriesAsync(query.From, query.To, query.Bucket, cancellationToken);
    }

    public async Task<IReadOnlyList<Alert>> Handle(GetAlertsRequiringAttentionQuery query, CancellationToken cancellationToken)
    {
        return await dashboardRepository.FindAlertsRequiringAttentionAsync(query.Take, cancellationToken);
    }

    public async Task<IReadOnlyList<ExpeditionActivitySummary>> Handle(GetActiveExpeditionsQuery query, CancellationToken cancellationToken)
    {
        return await dashboardRepository.FindActiveExpeditionsAsync(cancellationToken);
    }

    public async Task<TouristDashboard> Handle(GetTouristDashboardQuery query, CancellationToken cancellationToken)
    {
        var currentExpedition = await dashboardRepository.FindCurrentExpeditionForTouristAsync(query.TouristId, cancellationToken);
        var pastExpeditions = await dashboardRepository.FindPastExpeditionsForTouristAsync(
            query.TouristId, currentExpedition?.ExpeditionId ?? -1, cancellationToken);
        var alerts = await dashboardRepository.FindAlertsByTouristIdAsync(query.TouristId, cancellationToken);

        var activeAlerts = alerts.Where(a => a.IsActive()).ToList();
        var historicalAlerts = alerts.Where(a => !a.IsActive()).ToList();

        return new TouristDashboard(currentExpedition, activeAlerts, historicalAlerts, pastExpeditions);
    }
}
