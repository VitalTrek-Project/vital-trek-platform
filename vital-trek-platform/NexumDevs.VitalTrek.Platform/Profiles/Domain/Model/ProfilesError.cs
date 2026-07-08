namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model;

/// <summary>
/// Represents the base exception for the Profiles bounded context.
/// Encapsulates an error code defined in <see cref="Errors.ProfilesErrors"/>
/// that can be translated and mapped by the interface layer to HTTP status codes
/// and ProblemDetails responses.
/// </summary>
public class ProfilesError : Exception
{
    /// <summary>
    /// Gets the application-specific error code associated with the exception.
    /// </summary>
    public string ErrorCode { get; }

    public ProfilesError(string errorCode) : base(errorCode)
    {
        ErrorCode = errorCode;
    }

    public ProfilesError(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
