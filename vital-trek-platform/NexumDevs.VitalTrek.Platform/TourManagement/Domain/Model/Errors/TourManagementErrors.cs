namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;

/// <summary>
/// Centralized catalog of error keys used by the Tour Management bounded context.
/// These keys are intended to be used together with <c>IStringLocalizer</c>
/// to retrieve localized error messages from resource files.
/// </summary>
public static class TourManagementErrors
{
    /// <summary>
    /// Error raised when the requested tour cannot be found.
    /// </summary>
    public const string TourNotFound = "TourNotFound";

    /// <summary>
    /// Error raised when a tour with the same title already exists.
    /// </summary>
    public const string TourTitleAlreadyExists = "TourTitleAlreadyExists";

    /// <summary>
    /// Error raised when the specified tour capacity is invalid.
    /// </summary>
    public const string InvalidTourCapacity = "InvalidTourCapacity";

    /// <summary>
    /// Error raised when the tour cannot be updated in its current state.
    /// </summary>
    public const string TourCannotBeUpdated = "TourCannotBeUpdated";

    /// <summary>
    /// Error raised when the tour cannot be published.
    /// </summary>
    public const string TourCannotBePublished = "TourCannotBePublished";

    /// <summary>
    /// Error raised when the tour cannot be closed.
    /// </summary>
    public const string TourCannotBeClosed = "TourCannotBeClosed";

    /// <summary>
    /// Error raised when an already canceled tour is canceled again.
    /// </summary>
    public const string TourAlreadyCancelled = "TourAlreadyCancelled";

    /// <summary>
    /// Error raised when a tourist assignment is attempted on a tour
    /// that is not available for assignments.
    /// </summary>
    public const string TourNotAvailableForAssignment = "TourNotAvailableForAssignment";

    /// <summary>
    /// Error raised when the maximum tour capacity has been reached.
    /// </summary>
    public const string TourCapacityExceeded = "TourCapacityExceeded";

    /// <summary>
    /// Error raised when a tourist is already assigned to the tour.
    /// </summary>
    public const string TouristAlreadyAssigned = "TouristAlreadyAssigned";

    /// <summary>
    /// Error raised when a requested assignment cannot be found.
    /// </summary>
    public const string AssignmentNotFound = "AssignmentNotFound";

    /// <summary>
    /// Error raised when an already canceled assignment is canceled again
    /// or an invalid operation is performed on it.
    /// </summary>
    public const string AssignmentAlreadyCancelled = "AssignmentAlreadyCancelled";
}