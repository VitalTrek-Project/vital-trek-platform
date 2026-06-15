using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;

/// <summary>
/// Represents the assignment (reservation) of a tourist to a tour.
/// </summary>
public class TourAssignment
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected TourAssignment() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TourAssignment"/> class.
    /// </summary>
    /// <param name="tourId">The identifier of the assigned tour.</param>
    /// <param name="touristId">The identifier of the assigned tourist.</param>
    public TourAssignment(Guid tourId, Guid touristId)
    {
        Id = Guid.NewGuid();
        TourId = tourId;
        TouristId = touristId;
        Status = EAssignmentStatus.Pending;
        AssignedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of the assignment.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the assigned tour.
    /// </summary>
    public Guid TourId { get; private set; }

    /// <summary>
    /// Gets the identifier of the assigned tourist.
    /// </summary>
    public Guid TouristId { get; private set; }

    /// <summary>
    /// Gets the current status of the assignment.
    /// </summary>
    public EAssignmentStatus Status { get; private set; }

    /// <summary>
    /// Gets the date and time when the assignment was created.
    /// </summary>
    public DateTimeOffset AssignedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the assignment was confirmed.
    /// </summary>
    public DateTimeOffset? ConfirmedAt { get; private set; }

    /// <summary>
    /// Confirms the assignment.
    /// </summary>
    /// <returns>The current <see cref="TourAssignment"/> instance.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the assignment is not in pending status.
    /// </exception>
    public TourAssignment Confirm()
    {
        if (Status != EAssignmentStatus.Pending)
            throw new TourManagementError(
                TourManagementErrors.AssignmentAlreadyCancelled,
                "Only a pending assignment can be confirmed.");

        Status = EAssignmentStatus.Confirmed;
        ConfirmedAt = DateTimeOffset.UtcNow;
        return this;
    }

    /// <summary>
    /// Cancels the assignment.
    /// </summary>
    /// <returns>The current <see cref="TourAssignment"/> instance.</returns>
    /// <exception cref="TourManagementError">
    /// Thrown when the assignment has already been cancelled.
    /// </exception>
    public TourAssignment Cancel()
    {
        if (Status == EAssignmentStatus.Cancelled)
            throw new TourManagementError(
                TourManagementErrors.AssignmentAlreadyCancelled,
                "The assignment has already been cancelled.");

        Status = EAssignmentStatus.Cancelled;
        return this;
    }
}