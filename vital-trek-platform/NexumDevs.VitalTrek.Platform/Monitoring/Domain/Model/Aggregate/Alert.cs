using System;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

public partial class Alert : AuditableModel
{
    public Alert()
    {
        Type = default!;
        Severity = default!;
        Message = string.Empty;
    }

    public Alert(RaiseAlertCommand command)
    {
        ExpeditionId = command.ExpeditionId;
        TouristId = command.TouristId;
        Type = Enum.Parse<AlertType>(command.Type, true);
        Severity = Enum.Parse<AlertSeverity>(command.Severity, true);
        Message = command.Message;
        Status = AlertStatus.ACTIVE;
    }
    
    public Alert(int expeditionId, int touristId, AlertType type, AlertSeverity severity, string message)
    {
        ExpeditionId = expeditionId;
        TouristId = touristId;
        Type = type;
        Severity = severity;
        Message = message;
        Status = AlertStatus.ACTIVE;
    }

    public int Id { get; private set; }
    public int ExpeditionId { get; private set; }
    public int TouristId { get; private set; }
    public AlertType Type { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public AlertStatus Status { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset? AcknowledgedAt { get; private set; }
    public int? AcknowledgedBy { get; private set; }

    public bool IsActive() => Status == AlertStatus.ACTIVE;

    public void Acknowledge(int userId)
    {
        if (Status != AlertStatus.ACTIVE) return;
        Status = AlertStatus.ACKNOWLEDGED;
        AcknowledgedAt = DateTimeOffset.UtcNow;
        AcknowledgedBy = userId;
    }

    public void Dismiss()
    {
        if (Status == AlertStatus.DISMISSED) return;
        Status = AlertStatus.DISMISSED;
    }
}