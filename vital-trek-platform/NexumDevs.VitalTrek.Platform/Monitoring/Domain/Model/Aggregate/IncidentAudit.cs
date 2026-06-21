using NexumDevs.VitalTrek.Platform.Shared.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;

public partial class Incident : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}