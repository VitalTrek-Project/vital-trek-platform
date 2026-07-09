using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface IProgramCommandService
{
    Task<LoyaltyProgram> Handle(UpdateLoyaltyProgramCommand command, CancellationToken cancellationToken);
    Task<LoyaltyTier> Handle(CreateLoyaltyTierCommand command, CancellationToken cancellationToken);
    Task<LoyaltyTier> Handle(UpdateLoyaltyTierCommand command, CancellationToken cancellationToken);
    Task Handle(DeleteLoyaltyTierCommand command, CancellationToken cancellationToken);
}
