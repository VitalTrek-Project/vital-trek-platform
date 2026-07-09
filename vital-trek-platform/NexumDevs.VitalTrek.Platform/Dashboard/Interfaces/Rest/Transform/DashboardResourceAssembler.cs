using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Transform;

public static class DashboardResourceAssembler
{
    public static KpiResource ToResourceFromKpi(KpiValue kpi) =>
        new(kpi.Value, kpi.PreviousValue, kpi.DeltaPercentage);

    public static AdminDashboardSummaryResource ToResourceFromSummary(AdminDashboardSummary summary) =>
        new(
            ToResourceFromKpi(summary.ExpeditionsActive),
            ToResourceFromKpi(summary.ExpeditionsCompleted),
            ToResourceFromKpi(summary.AlertsOpen),
            summary.AlertsOpenBySeverity,
            ToResourceFromKpi(summary.TouristsActive),
            ToResourceFromKpi(summary.StaffAssigned));

    public static AlertsDistributionResource ToResourceFromDistribution(AlertsDistribution distribution) =>
        new(distribution.BySeverity, distribution.ByType);

    public static ExpeditionsTimeSeriesPointResource ToResourceFromTimeSeriesPoint(ExpeditionTimeSeriesPoint point) =>
        new(point.BucketStart, point.Count);

    public static AttentionAlertResource ToAttentionResourceFromEntity(Alert entity) =>
        new(
            entity.Id,
            entity.TouristId,
            entity.ExpeditionId,
            entity.Type.ToString(),
            entity.Severity.ToString(),
            entity.Message,
            entity.CreatedAt ?? DateTimeOffset.UtcNow);

    public static ActiveExpeditionResource ToResourceFromActivitySummary(ExpeditionActivitySummary summary) =>
        new(summary.ExpeditionId, summary.ExpeditionName, summary.Status, summary.GuideId, summary.TouristCount);

    public static TouristExpeditionResource ToResourceFromTouristActivity(TouristExpeditionActivity activity) =>
        new(activity.ExpeditionId, activity.ExpeditionName, activity.Status, activity.GuideId, activity.LastActivityAt);

    public static TouristAlertResource ToTouristAlertResourceFromEntity(Alert entity) =>
        new(
            entity.Id,
            entity.Type.ToString(),
            entity.Severity.ToString(),
            entity.Status.ToString(),
            entity.Message,
            entity.CreatedAt ?? DateTimeOffset.UtcNow);

    public static TouristDashboardResource ToResourceFromTouristDashboard(TouristDashboard dashboard) =>
        new(
            dashboard.CurrentExpedition != null ? ToResourceFromTouristActivity(dashboard.CurrentExpedition) : null,
            dashboard.ActiveAlerts.Select(ToTouristAlertResourceFromEntity),
            dashboard.HistoricalAlerts.Select(ToTouristAlertResourceFromEntity),
            dashboard.PastExpeditions.Select(ToResourceFromTouristActivity));
}
