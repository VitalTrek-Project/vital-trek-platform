using NexumDevs.VitalTrek.Platform.Shared.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Expedition : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
