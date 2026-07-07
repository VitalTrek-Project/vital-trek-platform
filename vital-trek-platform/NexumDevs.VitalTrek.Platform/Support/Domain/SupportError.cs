namespace NexumDevs.VitalTrek.Platform.Support.Domain;

/// <summary>
/// Represents the base exception for the Support bounded context.
/// Encapsulates an error code defined in <see cref="Model.Errors.SupportErrors"/>
/// that can be translated and mapped by the interface layer to HTTP status codes
/// and ProblemDetails responses.
/// </summary>
public class SupportError : Exception
{
    /// <summary>
    /// Gets the application-specific error code associated with the exception.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportError"/> class
    /// using the specified error code as the exception message.
    /// </summary>
    /// <param name="errorCode">The application-specific error code.</param>
    public SupportError(string errorCode) : base(errorCode)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportError"/> class
    /// using the specified error code and custom message.
    /// </summary>
    /// <param name="errorCode">The application-specific error code.</param>
    /// <param name="message">The error message describing the exception.</param>
    public SupportError(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
