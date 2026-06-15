namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Errors;

/// <summary>
/// Catálogo centralizado de claves de error del BC TourManagement.
/// Estas claves se usan junto con IStringLocalizer para obtener
/// el mensaje traducido (ver Resources/TourManagementMessages.resx).
/// Equivalente a "PublishingErrors.cs".
/// </summary>
public static class TourManagementErrors
{
    public const string TourNotFound = "TourNotFound";
    public const string TourTitleAlreadyExists = "TourTitleAlreadyExists";
    public const string InvalidTourCapacity = "InvalidTourCapacity";
    public const string TourCannotBeUpdated = "TourCannotBeUpdated";
    public const string TourCannotBePublished = "TourCannotBePublished";
    public const string TourCannotBeClosed = "TourCannotBeClosed";
    public const string TourAlreadyCancelled = "TourAlreadyCancelled";
    public const string TourNotAvailableForAssignment = "TourNotAvailableForAssignment";
    public const string TourCapacityExceeded = "TourCapacityExceeded";
    public const string TouristAlreadyAssigned = "TouristAlreadyAssigned";
    public const string AssignmentNotFound = "AssignmentNotFound";
    public const string AssignmentAlreadyCancelled = "AssignmentAlreadyCancelled";
}