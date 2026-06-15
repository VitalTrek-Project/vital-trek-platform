using Microsoft.Extensions.Logging;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.EventHandlers;

/// <summary>
/// Manejador del evento de dominio TourPublishedEvent.
/// Equivalente a "CategoryCreatedEventHandler" en Publishing.
///
/// Aquí podrías, por ejemplo, notificar a otro Bounded Context
/// (Notifications, Search/Indexing, etc.) cuando un tour se publica.
/// </summary>
public class TourPublishedEventHandler
{
    private readonly ILogger<TourPublishedEventHandler> _logger;

    public TourPublishedEventHandler(ILogger<TourPublishedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TourPublishedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Tour {TourId} de la agencia {AgencyId} fue publicado el {PublishedAt}.",
            domainEvent.TourId, domainEvent.AgencyId, domainEvent.PublishedAt);

        // Ejemplo: publicar un mensaje a otro BC (Notifications, Search, etc.)
        // await _eventBus.PublishAsync(domainEvent, cancellationToken);

        return Task.CompletedTask;
    }
}