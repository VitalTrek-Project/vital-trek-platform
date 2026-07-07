namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record AdminDashboardSummary(
    KpiValue ExpeditionsActive,
    KpiValue ExpeditionsCompleted,
    KpiValue AlertsOpen,
    IDictionary<string, int> AlertsOpenBySeverity,
    KpiValue TouristsActive,
    KpiValue StaffAssigned);
