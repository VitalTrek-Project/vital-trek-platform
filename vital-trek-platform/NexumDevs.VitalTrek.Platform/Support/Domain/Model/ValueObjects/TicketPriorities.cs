namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.ValueObjects;

/// <summary>
/// Defines the allowed priority values for a <see cref="Aggregates.Ticket"/>.
/// </summary>
public static class TicketPriorities
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";
    public const string Urgent = "Urgent";

    private static readonly IReadOnlySet<string> All = new HashSet<string> { Low, Medium, High, Urgent };

    /// <summary>
    /// Determines whether the specified value is one of the recognized ticket priorities.
    /// </summary>
    public static bool IsValid(string? priority) => priority is not null && All.Contains(priority);
}
