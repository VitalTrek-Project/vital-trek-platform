using Microsoft.Extensions.Logging;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="TourPublishedEvent"/> domain event.
/// </summary>
/// <remarks>
/// This event handler is responsible for processing actions that should occur
/// after a tour has been published, such as notifying other bounded contexts,
/// updating search indexes, or triggering external integrations.
/// </remarks>
public class TourPublishedEventHandler
{
    private readonly ILogger<TourPublishedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TourPublishedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">
    /// Logger used to record information related to tour publication events.
    /// </param>
    public TourPublishedEventHandler(ILogger<TourPublishedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the <see cref="TourPublishedEvent"/>.
    /// </summary>
    /// <param name="domainEvent">
    /// The domain event containing information about the published tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A completed task representing the asynchronous handling operation.
    /// </returns>
    public Task Handle(TourPublishedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Tour {TourId} from agency {AgencyId} was published on {PublishedAt}.",
            domainEvent.TourId, domainEvent.AgencyId, domainEvent.PublishedAt);

        // Example: publish an integration event to another bounded context
        // (Notifications, Search, Analytics, etc.).
        // await _eventBus.PublishAsync(domainEvent, cancellationToken);

        return Task.CompletedTask;
    }
}