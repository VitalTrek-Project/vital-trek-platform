namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record AdminDashboardSummaryResource(
    KpiResource ExpeditionsActive,
    KpiResource ExpeditionsCompleted,
    KpiResource AlertsOpen,
    IDictionary<string, int> AlertsOpenBySeverity,
    KpiResource TouristsActive,
    KpiResource StaffAssigned);
