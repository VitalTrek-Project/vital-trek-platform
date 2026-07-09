using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;
using NexumDevs.VitalTrek.Platform.Dashboard.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace NexumDevs.VitalTrek.Platform.Dashboard.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Read-only aggregation repository backing the Dashboard bounded context.
/// Injects AppDbContext directly (as every other infrastructure repository does) and
/// pushes GroupBy/Where/Select aggregation down to MySQL wherever the provider can
/// translate it; only the small already-aggregated results are materialized in memory.
/// </summary>
public class DashboardRepository(AppDbContext context) : IDashboardRepository
{
    private readonly AppDbContext _context = context;

    public async Task<int> CountExpeditionsByStatusKeyAsync(string statusKey, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
    {
        var query = _context.Set<Expedition>()
            .Where(e => e.CreatedAt != null && e.CreatedAt >= from && e.CreatedAt <= to);

        return await FilterByStatusKey(query, statusKey).CountAsync(cancellationToken);
    }

    public async Task<int> CountActiveAlertsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
    {
        return await _context.Set<Alert>()
            .Where(a => a.Status == AlertStatus.ACTIVE && a.CreatedAt != null && a.CreatedAt >= from && a.CreatedAt <= to)
            .CountAsync(cancellationToken);
    }

    public async Task<IDictionary<string, int>> CountAlertsBySeverityAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
    {
        var rows = await _context.Set<Alert>()
            .Where(a => a.CreatedAt != null && a.CreatedAt >= from && a.CreatedAt <= to)
            .GroupBy(a => a.Severity)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(r => r.Key.ToString(), r => r.Count);
    }

    public async Task<IDictionary<string, int>> CountAlertsByTypeAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
    {
        var rows = await _context.Set<Alert>()
            .Where(a => a.CreatedAt != null && a.CreatedAt >= from && a.CreatedAt <= to)
            .GroupBy(a => a.Type)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(r => r.Key.ToString(), r => r.Count);
    }

    public async Task<int> CountDistinctActiveTouristsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
    {
        var activeExpeditionIds = await FilterByStatusKey(_context.Set<Expedition>(), "active")
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        if (activeExpeditionIds.Count == 0) return 0;

        var fromUtc = from.UtcDateTime;
        var toUtc = to.UtcDateTime;

        var vitalTourists = await _context.Set<VitalSignReading>()
            .Where(v => activeExpeditionIds.Contains(v.ExpeditionId) && v.RecordedAt >= fromUtc && v.RecordedAt <= toUtc)
            .Select(v => v.TouristId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var locationTourists = await _context.Set<LocationReading>()
            .Where(l => activeExpeditionIds.Contains(l.ExpeditionId) && l.RecordedAt >= fromUtc && l.RecordedAt <= toUtc)
            .Select(l => l.TouristId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return vitalTourists.Union(locationTourists).Distinct().Count();
    }

    public async Task<int> CountDistinctGuidesAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
    {
        return await _context.Set<Expedition>()
            .Where(e => e.CreatedAt != null && e.CreatedAt >= from && e.CreatedAt <= to)
            .Select(e => e.GuideID)
            .Distinct()
            .CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExpeditionTimeSeriesPoint>> GetExpeditionsTimeSeriesAsync(DateTimeOffset from, DateTimeOffset to, string bucket, CancellationToken cancellationToken)
    {
        var createdDates = await _context.Set<Expedition>()
            .Where(e => e.CreatedAt != null && e.CreatedAt >= from && e.CreatedAt <= to)
            .Select(e => e.CreatedAt!.Value)
            .ToListAsync(cancellationToken);

        var useMonth = string.Equals(bucket, "month", StringComparison.OrdinalIgnoreCase);

        return createdDates
            .GroupBy(date => useMonth ? StartOfMonth(date) : StartOfWeek(date))
            .Select(g => new ExpeditionTimeSeriesPoint(g.Key, g.Count()))
            .OrderBy(p => p.BucketStart)
            .ToList();
    }

    public async Task<IReadOnlyList<Alert>> FindAlertsRequiringAttentionAsync(int take, CancellationToken cancellationToken)
    {
        return await _context.Set<Alert>()
            .Where(a => a.Status == AlertStatus.ACTIVE)
            .OrderByDescending(a => a.Severity)
            .ThenBy(a => a.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExpeditionActivitySummary>> FindActiveExpeditionsAsync(CancellationToken cancellationToken)
    {
        var activeExpeditions = await FilterByStatusKey(_context.Set<Expedition>(), "active")
            .Select(e => new { e.Id, e.ExpeditionName, e.Status, e.GuideID })
            .ToListAsync(cancellationToken);

        if (activeExpeditions.Count == 0) return Array.Empty<ExpeditionActivitySummary>();

        var expeditionIds = activeExpeditions.Select(e => e.Id).ToList();

        var touristsFromVitals = await _context.Set<VitalSignReading>()
            .Where(v => expeditionIds.Contains(v.ExpeditionId))
            .Select(v => new { v.ExpeditionId, v.TouristId })
            .Distinct()
            .ToListAsync(cancellationToken);

        var touristsFromLocations = await _context.Set<LocationReading>()
            .Where(l => expeditionIds.Contains(l.ExpeditionId))
            .Select(l => new { l.ExpeditionId, l.TouristId })
            .Distinct()
            .ToListAsync(cancellationToken);

        var touristCountByExpedition = touristsFromVitals.Concat(touristsFromLocations)
            .GroupBy(x => x.ExpeditionId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.TouristId).Distinct().Count());

        return activeExpeditions
            .Select(e => new ExpeditionActivitySummary(
                e.Id,
                e.ExpeditionName,
                e.Status,
                e.GuideID,
                touristCountByExpedition.TryGetValue(e.Id, out var count) ? count : 0))
            .ToList();
    }

    public async Task<TouristExpeditionActivity?> FindCurrentExpeditionForTouristAsync(int touristId, CancellationToken cancellationToken)
    {
        var lastVital = await _context.Set<VitalSignReading>()
            .Where(v => v.TouristId == touristId)
            .OrderByDescending(v => v.RecordedAt)
            .Select(v => new { v.ExpeditionId, v.RecordedAt })
            .FirstOrDefaultAsync(cancellationToken);

        var lastLocation = await _context.Set<LocationReading>()
            .Where(l => l.TouristId == touristId)
            .OrderByDescending(l => l.RecordedAt)
            .Select(l => new { l.ExpeditionId, l.RecordedAt })
            .FirstOrDefaultAsync(cancellationToken);

        var candidates = new List<(int ExpeditionId, DateTime RecordedAt)>();
        if (lastVital != null) candidates.Add((lastVital.ExpeditionId, lastVital.RecordedAt));
        if (lastLocation != null) candidates.Add((lastLocation.ExpeditionId, lastLocation.RecordedAt));

        if (candidates.Count == 0) return null;

        var latest = candidates.OrderByDescending(c => c.RecordedAt).First();

        var expedition = await _context.Set<Expedition>()
            .FirstOrDefaultAsync(e => e.Id == latest.ExpeditionId, cancellationToken);

        if (expedition == null || NormalizeStatusKey(expedition.Status) == "finished") return null;

        return new TouristExpeditionActivity(
            expedition.Id,
            expedition.ExpeditionName,
            expedition.Status,
            expedition.GuideID,
            new DateTimeOffset(DateTime.SpecifyKind(latest.RecordedAt, DateTimeKind.Utc)));
    }

    public async Task<IReadOnlyList<TouristExpeditionActivity>> FindPastExpeditionsForTouristAsync(int touristId, int excludeExpeditionId, CancellationToken cancellationToken)
    {
        var vitalExpeditions = await _context.Set<VitalSignReading>()
            .Where(v => v.TouristId == touristId)
            .GroupBy(v => v.ExpeditionId)
            .Select(g => new { ExpeditionId = g.Key, LastActivityAt = g.Max(v => v.RecordedAt) })
            .ToListAsync(cancellationToken);

        var locationExpeditions = await _context.Set<LocationReading>()
            .Where(l => l.TouristId == touristId)
            .GroupBy(l => l.ExpeditionId)
            .Select(g => new { ExpeditionId = g.Key, LastActivityAt = g.Max(l => l.RecordedAt) })
            .ToListAsync(cancellationToken);

        var lastActivityByExpedition = vitalExpeditions.Concat(locationExpeditions)
            .GroupBy(x => x.ExpeditionId)
            .Where(g => g.Key != excludeExpeditionId)
            .ToDictionary(g => g.Key, g => g.Max(x => x.LastActivityAt));

        if (lastActivityByExpedition.Count == 0) return Array.Empty<TouristExpeditionActivity>();

        var expeditionIds = lastActivityByExpedition.Keys.ToList();
        var expeditions = await _context.Set<Expedition>()
            .Where(e => expeditionIds.Contains(e.Id))
            .ToListAsync(cancellationToken);

        return expeditions
            .Select(e => new TouristExpeditionActivity(
                e.Id,
                e.ExpeditionName,
                e.Status,
                e.GuideID,
                new DateTimeOffset(DateTime.SpecifyKind(lastActivityByExpedition[e.Id], DateTimeKind.Utc))))
            .OrderByDescending(e => e.LastActivityAt)
            .ToList();
    }

    public async Task<IReadOnlyList<Alert>> FindAlertsByTouristIdAsync(int touristId, CancellationToken cancellationToken)
    {
        return await _context.Set<Alert>()
            .Where(a => a.TouristId == touristId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Expedition.Status has no enum (free-text column). This mirrors, key for key, the
    /// frontend's getExpeditionStatusKey (src/navigation/presentation/utils/navigation-presenter.js)
    /// so admin/tourist dashboards agree with the rest of the app on what "active" means.
    /// Expressed with .ToLower() comparisons so EF Core can translate it to SQL instead of
    /// pulling the whole Expeditions table into memory.
    /// </summary>
    private static IQueryable<Expedition> FilterByStatusKey(IQueryable<Expedition> query, string statusKey)
    {
        return statusKey switch
        {
            "active" => query.Where(e => e.Status.ToLower() == "in_progress"),
            "finished" => query.Where(e => e.Status.ToLower() == "finished"),
            "planned" => query.Where(e => e.Status.ToLower() == "planned" || e.Status.ToLower() == "pending"),
            _ => query.Where(e =>
                e.Status.ToLower() != "in_progress" &&
                e.Status.ToLower() != "finished" &&
                e.Status.ToLower() != "planned" &&
                e.Status.ToLower() != "pending")
        };
    }

    private static string NormalizeStatusKey(string status)
    {
        var value = (status ?? string.Empty).Trim().ToLowerInvariant();
        if (value == "in_progress") return "active";
        if (value == "finished") return "finished";
        if (value == "planned" || value == "pending") return "planned";
        return "neutral";
    }

    private static DateTimeOffset StartOfWeek(DateTimeOffset date)
    {
        var diff = (7 + (int)date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
        return new DateTimeOffset(date.Date.AddDays(-diff), TimeSpan.Zero);
    }

    private static DateTimeOffset StartOfMonth(DateTimeOffset date)
    {
        return new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
