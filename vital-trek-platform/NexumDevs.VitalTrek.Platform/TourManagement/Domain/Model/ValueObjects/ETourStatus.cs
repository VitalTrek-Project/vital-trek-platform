namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

/// <summary>
/// Represents the lifecycle status of a tour.
/// </summary>
public enum ETourStatus
{
    /// <summary>
    /// The tour is being created or edited and is not yet available to tourists.
    /// </summary>
    Draft,

    /// <summary>
    /// The tour has been published and is available for tourist assignments.
    /// </summary>
    Available,

    /// <summary>
    /// The tour is closed and no longer accepts new tourist assignments.
    /// </summary>
    Closed,

    /// <summary>
    /// The tour has been canceled and is no longer active.
    /// </summary>
    Cancelled
}