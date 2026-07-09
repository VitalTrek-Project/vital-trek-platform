using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface IRedemptionCommandService
{
    Task<Redemption> Handle(RedeemRewardCommand command, CancellationToken cancellationToken);
    Task<Redemption> Handle(MarkRedemptionUsedCommand command, CancellationToken cancellationToken);
}
