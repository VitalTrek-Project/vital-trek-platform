namespace NexumDevs.VitalTrek.Platform.Engagement.Domain;

/// <summary>
/// Represents the base exception for the Engagement bounded context.
/// Encapsulates an error code defined in <see cref="Model.Errors.EngagementErrors"/>
/// that can be translated and mapped by the interface layer to HTTP status codes
/// and ProblemDetails responses.
/// </summary>
public class EngagementError : Exception
{
    /// <summary>
    /// Gets the application-specific error code associated with the exception.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EngagementError"/> class
    /// using the specified error code as the exception message.
    /// </summary>
    /// <param name="errorCode">The application-specific error code.</param>
    public EngagementError(string errorCode) : base(errorCode)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EngagementError"/> class
    /// using the specified error code and custom message.
    /// </summary>
    /// <param name="errorCode">The application-specific error code.</param>
    /// <param name="message">The error message describing the exception.</param>
    public EngagementError(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
