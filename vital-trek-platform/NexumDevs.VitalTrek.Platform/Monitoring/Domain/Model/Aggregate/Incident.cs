using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
//using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

public partial class Incident 
{
    public Incident()
       
    {
        ExpeditionId = 0;
        ReportedBy = 0;
        Description = null!;
        Severity = null!;
        Status = null!;
        ReportedAt = null!;

    }

    public Incident(CreateIncidentCommand command) 
    {
        ArgumentNullException.ThrowIfNull(command);
        ExpeditionId = command.ExpeditionId;
        ReportedBy = command.ReportedBy;
        Description = command.Description;
        Severity = command.Severity;
        Status = command.Status;
        ReportedAt = command.ReportedAt;

    }



    public int Id { get; private set; }

    public int ExpeditionId { get; private set; }
    public int ReportedBy { get; private set; }
    public string Description { get; private set; }
    public string Severity { get; private set; }
    public string Status { get; private set; }
    public string ReportedAt { get; private set; }

}