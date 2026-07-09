using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface IBadgeCommandService
{
    Task<BadgeDefinition> Handle(CreateBadgeDefinitionCommand command, CancellationToken cancellationToken);
}
