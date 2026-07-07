namespace NexumDevs.VitalTrek.Platform.Support.Domain.Model.Errors;

/// <summary>
/// Centralized catalog of error keys used by the Support bounded context.
/// These keys are intended to be used together with <c>IStringLocalizer</c>
/// to retrieve localized error messages from resource files.
/// </summary>
public static class SupportErrors
{
    /// <summary>
    /// Error raised when the requested support ticket cannot be found.
    /// </summary>
    public const string TicketNotFound = "TicketNotFound";

    /// <summary>
    /// Error raised when the user mode provided for a ticket is not a recognized value.
    /// </summary>
    public const string InvalidUserMode = "InvalidUserMode";

    /// <summary>
    /// Error raised when the author mode provided for a reply is not a recognized value.
    /// </summary>
    public const string InvalidAuthorMode = "InvalidAuthorMode";

    /// <summary>
    /// Error raised when the priority provided is not a recognized value.
    /// </summary>
    public const string InvalidPriority = "InvalidPriority";

    /// <summary>
    /// Error raised when the status provided is not a recognized value.
    /// </summary>
    public const string InvalidStatus = "InvalidStatus";

    /// <summary>
    /// Error raised when required ticket fields are missing or empty.
    /// </summary>
    public const string InvalidTicketData = "InvalidTicketData";

    /// <summary>
    /// Error raised when a reply is submitted with an empty message.
    /// </summary>
    public const string EmptyReplyMessage = "EmptyReplyMessage";

    /// <summary>
    /// Error raised when a ticket update is requested without providing any field to update.
    /// </summary>
    public const string NoUpdateFieldsProvided = "NoUpdateFieldsProvided";
}
