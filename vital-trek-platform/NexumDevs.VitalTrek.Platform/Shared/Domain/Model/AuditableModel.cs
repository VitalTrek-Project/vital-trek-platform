using NexumDevs.VitalTrek.Platform.Shared.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

public abstract class AuditableModel : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
