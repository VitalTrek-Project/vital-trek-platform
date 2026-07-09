namespace NexumDevs.VitalTrek.Platform.Dashboard.Interfaces.Rest.Resources;

public record TouristDashboardResource(
    TouristExpeditionResource? CurrentExpedition,
    IEnumerable<TouristAlertResource> ActiveAlerts,
    IEnumerable<TouristAlertResource> HistoricalAlerts,
    IEnumerable<TouristExpeditionResource> PastExpeditions);
