using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

public class ProgramCommandService(
    ILoyaltyProgramRepository programRepository,
    ILoyaltyTierRepository tierRepository,
    IUnitOfWork unitOfWork) : IProgramCommandService
{
    public async Task<LoyaltyProgram> Handle(UpdateLoyaltyProgramCommand command, CancellationToken cancellationToken)
    {
        var program = await programRepository.FindByAgencyIdAsync(command.AgencyId, cancellationToken);

        if (program is null)
        {
            program = LoyaltyProgram.CreateDefault(command.AgencyId);
            await programRepository.AddAsync(program, cancellationToken);
        }

        program.UpdateSettings(
            command.PointsPerExpeditionCompleted,
            command.PointsPerExpeditionBooked,
            command.PointsPerReferral,
            command.PointsPerReview,
            command.ExpirationMonths);

        programRepository.Update(program);
        await unitOfWork.CompleteAsync(cancellationToken);
        return program;
    }

    public async Task<LoyaltyTier> Handle(CreateLoyaltyTierCommand command, CancellationToken cancellationToken)
    {
        var tier = new LoyaltyTier(command.AgencyId, command.Name, command.MinPoints, command.Benefits, command.SortOrder);
        await tierRepository.AddAsync(tier, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return tier;
    }

    public async Task<LoyaltyTier> Handle(UpdateLoyaltyTierCommand command, CancellationToken cancellationToken)
    {
        var tier = await tierRepository.FindByIdAndAgencyAsync(command.TierId, command.AgencyId, cancellationToken)
                   ?? throw new EngagementError(EngagementErrors.TierNotFound);

        tier.Update(command.Name, command.MinPoints, command.Benefits, command.SortOrder);
        tierRepository.Update(tier);
        await unitOfWork.CompleteAsync(cancellationToken);
        return tier;
    }

    public async Task Handle(DeleteLoyaltyTierCommand command, CancellationToken cancellationToken)
    {
        var tier = await tierRepository.FindByIdAndAgencyAsync(command.TierId, command.AgencyId, cancellationToken)
                   ?? throw new EngagementError(EngagementErrors.TierNotFound);

        tierRepository.Remove(tier);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
