using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

public class RewardCommandService(IRewardRepository rewardRepository, IUnitOfWork unitOfWork) : IRewardCommandService
{
    public async Task<Reward> Handle(CreateRewardCommand command, CancellationToken cancellationToken)
    {
        var reward = new Reward(command.AgencyId, command.Name, command.Description, command.PointsCost, command.Stock);
        await rewardRepository.AddAsync(reward, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return reward;
    }

    public async Task<Reward> Handle(UpdateRewardCommand command, CancellationToken cancellationToken)
    {
        var reward = await rewardRepository.FindByIdAndAgencyAsync(command.RewardId, command.AgencyId, cancellationToken)
                     ?? throw new EngagementError(EngagementErrors.RewardNotFound);

        reward.Update(command.Name, command.Description, command.PointsCost, command.Stock);
        reward.SetActive(command.IsActive);

        rewardRepository.Update(reward);
        await unitOfWork.CompleteAsync(cancellationToken);
        return reward;
    }

    public async Task Handle(DeactivateRewardCommand command, CancellationToken cancellationToken)
    {
        var reward = await rewardRepository.FindByIdAndAgencyAsync(command.RewardId, command.AgencyId, cancellationToken)
                     ?? throw new EngagementError(EngagementErrors.RewardNotFound);

        reward.SetActive(false);
        rewardRepository.Update(reward);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
