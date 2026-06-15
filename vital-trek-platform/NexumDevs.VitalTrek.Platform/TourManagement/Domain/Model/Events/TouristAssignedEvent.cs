namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Events;

/// <summary>
/// Evento de dominio que se dispara cuando un turista es asignado a un Tour.
/// </summary>
public record TouristAssignedEvent(Guid TourId, Guid TouristId, DateTimeOffset AssignedAt);