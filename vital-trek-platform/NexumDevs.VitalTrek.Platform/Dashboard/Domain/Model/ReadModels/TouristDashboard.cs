using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record TouristDashboard(
    TouristExpeditionActivity? CurrentExpedition,
    IReadOnlyList<Alert> ActiveAlerts,
    IReadOnlyList<Alert> HistoricalAlerts,
    IReadOnlyList<TouristExpeditionActivity> PastExpeditions);
