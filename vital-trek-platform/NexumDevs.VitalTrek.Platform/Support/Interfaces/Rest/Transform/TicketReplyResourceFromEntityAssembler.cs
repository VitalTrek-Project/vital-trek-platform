using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Support.Interfaces.Rest.Transform;

public static class TicketReplyResourceFromEntityAssembler
{
    public static TicketReplyResource ToResourceFromEntity(TicketReply entity)
    {
        return new TicketReplyResource(
            entity.Id,
            entity.TicketId,
            entity.AuthorName,
            entity.AuthorMode,
            entity.Message,
            entity.CreatedAt);
    }
}
