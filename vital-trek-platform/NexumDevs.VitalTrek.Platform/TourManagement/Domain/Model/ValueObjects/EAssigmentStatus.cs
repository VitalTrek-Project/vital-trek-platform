namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the status of a tourist assignment within a tour.
/// </summary>
public enum EAssignmentStatus
{
    /// <summary>
    /// The assignment has been created but has not yet been confirmed.
    /// </summary>
    Pending,

    /// <summary>
    /// The assignment has been confirmed.
    /// </summary>
    Confirmed,

    /// <summary>
    /// The assignment has been canceled.
    /// </summary>
    Cancelled
}