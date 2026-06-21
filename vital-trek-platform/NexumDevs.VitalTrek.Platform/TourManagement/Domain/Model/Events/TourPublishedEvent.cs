namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;

/// <summary>
/// Domain event raised when a tour transitions from the Draft state
/// to the Available state.
/// </summary>
/// <remarks>
/// This event can be used to trigger actions in other bounded contexts,
/// such as notifications, search indexing, analytics, or integrations.
/// </remarks>
/// <param name="TourId">
/// The unique identifier of the published tour.
/// </param>
/// <param name="AgencyId">
/// The unique identifier of the agency that owns the published tour.
/// </param>
/// <param name="PublishedAt">
/// The date and time when the tour was published.
/// </param>
public record TourPublishedEvent(
    Guid TourId,
    Guid AgencyId,
    DateTimeOffset PublishedAt);