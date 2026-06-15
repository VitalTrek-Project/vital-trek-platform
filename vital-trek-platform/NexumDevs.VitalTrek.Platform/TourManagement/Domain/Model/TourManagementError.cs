namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model;

/// <summary>
/// Excepción base del BC TourManagement. Encapsula una clave de error
/// (definida en TourManagementErrors) que la capa de Interfaces puede
/// traducir y mapear a un código HTTP / ProblemDetails.
/// Equivalente a "PublishingError.cs".
/// </summary>
public class TourManagementError : Exception
{
    public string ErrorCode { get; }

    public TourManagementError(string errorCode) : base(errorCode)
    {
        ErrorCode = errorCode;
    }

    public TourManagementError(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}