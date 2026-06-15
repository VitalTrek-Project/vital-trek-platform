namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the difficulty level of a tour.
/// </summary>
public enum EDifficultyLevel
{
    /// <summary>
    /// Suitable for beginners and participants with little or no experience.
    /// </summary>
    Easy,

    /// <summary>
    /// Requires a moderate level of physical effort or experience.
    /// </summary>
    Moderate,

    /// <summary>
    /// Requires significant physical effort, endurance, or experience.
    /// </summary>
    Hard,

    /// <summary>
    /// Intended for highly experienced participants and demanding conditions.
    /// </summary>
    Expert
}