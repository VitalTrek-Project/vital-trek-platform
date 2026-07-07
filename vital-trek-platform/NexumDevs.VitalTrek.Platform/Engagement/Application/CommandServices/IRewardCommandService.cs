using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface IRewardCommandService
{
    Task<Reward> Handle(CreateRewardCommand command, CancellationToken cancellationToken);
    Task<Reward> Handle(UpdateRewardCommand command, CancellationToken cancellationToken);
    Task Handle(DeactivateRewardCommand command, CancellationToken cancellationToken);
}
