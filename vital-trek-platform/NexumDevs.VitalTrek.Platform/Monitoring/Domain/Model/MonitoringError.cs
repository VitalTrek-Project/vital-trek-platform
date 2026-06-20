namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model;

public enum MonitoringError
{
    None,
    IncidentNotFound,
    AlertNotFound,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}