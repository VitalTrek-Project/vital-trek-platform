namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;

/// <summary>
/// Evento de dominio que se dispara cuando un Tour pasa de Draft a Available.
/// Equivalente a "CategoryCreatedEvent" en Publishing.
/// </summary>
public record TourPublishedEvent(Guid TourId, Guid AgencyId, DateTimeOffset PublishedAt);