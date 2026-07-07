using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

public class BadgeCommandService(IBadgeDefinitionRepository badgeDefinitionRepository, IUnitOfWork unitOfWork) : IBadgeCommandService
{
    public async Task<BadgeDefinition> Handle(CreateBadgeDefinitionCommand command, CancellationToken cancellationToken)
    {
        if (await badgeDefinitionRepository.ExistsByCodeAsync(command.Code, cancellationToken))
            throw new EngagementError(EngagementErrors.DuplicateBadgeCode);

        var badge = new BadgeDefinition(command.AgencyId, command.Code, command.Name, command.Description, command.RuleType, command.RuleThreshold);
        await badgeDefinitionRepository.AddAsync(badge, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return badge;
    }
}
