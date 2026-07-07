namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.ValueObjects;

/// <summary>
/// Defines the allowed lifecycle values for a <see cref="Aggregates.Ticket"/> status.
/// </summary>
public static class TicketStatuses
{
    public const string Open = "Open";
    public const string InProgress = "InProgress";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";

    private static readonly IReadOnlySet<string> All = new HashSet<string> { Open, InProgress, Resolved, Closed };

    /// <summary>
    /// Determines whether the specified value is one of the recognized ticket statuses.
    /// </summary>
    public static bool IsValid(string? status) => status is not null && All.Contains(status);
}
