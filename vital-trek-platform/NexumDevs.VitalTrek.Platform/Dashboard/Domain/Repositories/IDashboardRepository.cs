using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Repositories;

/// <summary>
/// Read-only aggregation repository for the Dashboard bounded context.
/// Unlike other bounded contexts, this does not extend IBaseRepository&lt;TEntity&gt;
/// because it aggregates across multiple existing aggregates (Alert, Expedition,
/// LocationReading, VitalSignReading) instead of owning a single one.
/// </summary>
public interface IDashboardRepository
{
    /// <summary>
    /// Counts expeditions created within [from, to] whose normalized status equals
    /// "active", "finished", "planned" or "neutral" (see DashboardRepository.NormalizeStatusKey,
    /// which mirrors the frontend's getExpeditionStatusKey since Expedition.Status has no enum).
    /// </summary>
    Task<int> CountExpeditionsByStatusKeyAsync(string statusKey, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

    Task<int> CountActiveAlertsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

    Task<IDictionary<string, int>> CountAlertsBySeverityAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

    Task<IDictionary<string, int>> CountAlertsByTypeAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

    /// <summary>Distinct tourists with location/vital-sign readings in active expeditions within [from, to].</summary>
    Task<int> CountDistinctActiveTouristsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

    /// <summary>Distinct guides (GuideID) of expeditions created within [from, to].</summary>
    Task<int> CountDistinctGuidesAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpeditionTimeSeriesPoint>> GetExpeditionsTimeSeriesAsync(DateTimeOffset from, DateTimeOffset to, string bucket, CancellationToken cancellationToken);

    Task<IReadOnlyList<Alert>> FindAlertsRequiringAttentionAsync(int take, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpeditionActivitySummary>> FindActiveExpeditionsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Infers the tourist's current expedition from their most recent location/vital-sign
    /// reading, since there is no explicit tourist-expedition enrollment table today.
    /// Returns null if the tourist has no activity, or their most recent expedition is finished.
    /// </summary>
    Task<TouristExpeditionActivity?> FindCurrentExpeditionForTouristAsync(int touristId, CancellationToken cancellationToken);

    Task<IReadOnlyList<TouristExpeditionActivity>> FindPastExpeditionsForTouristAsync(int touristId, int excludeExpeditionId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Alert>> FindAlertsByTouristIdAsync(int touristId, CancellationToken cancellationToken);
}
