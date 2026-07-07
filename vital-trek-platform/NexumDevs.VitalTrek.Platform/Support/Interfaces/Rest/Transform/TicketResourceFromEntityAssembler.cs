using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Transform;

public static class TicketResourceFromEntityAssembler
{
    public static TicketResource ToResourceFromEntity(Ticket entity)
    {
        return new TicketResource(
            entity.Id,
            entity.UserId,
            entity.UserMode,
            entity.FullName,
            entity.Email,
            entity.Subject,
            entity.Category,
            entity.Description,
            entity.Priority,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
