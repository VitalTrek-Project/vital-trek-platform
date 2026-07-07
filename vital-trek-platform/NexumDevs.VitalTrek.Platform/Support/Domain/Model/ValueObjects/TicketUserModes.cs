namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.ValueObjects;

/// <summary>
/// Defines the allowed actor roles that can open a <see cref="Aggregates.Ticket"/>
/// or author a <see cref="Entities.TicketReply"/>.
/// </summary>
public static class TicketUserModes
{
    public const string Tourist = "Tourist";
    public const string Guide = "Guide";
    public const string Support = "Support";

    private static readonly IReadOnlySet<string> All = new HashSet<string> { Tourist, Guide, Support };

    /// <summary>
    /// Determines whether the specified value is one of the recognized user modes.
    /// </summary>
    public static bool IsValid(string? userMode) => userMode is not null && All.Contains(userMode);
}
