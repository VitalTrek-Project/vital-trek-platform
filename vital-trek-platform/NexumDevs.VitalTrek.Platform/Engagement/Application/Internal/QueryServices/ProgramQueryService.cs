using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;

public class ProgramQueryService(
    ILoyaltyProgramRepository programRepository,
    ILoyaltyTierRepository tierRepository,
    IUnitOfWork unitOfWork) : IProgramQueryService
{
    public async Task<LoyaltyProgram> Handle(GetLoyaltyProgramQuery query, CancellationToken cancellationToken)
    {
        var program = await programRepository.FindByAgencyIdAsync(query.AgencyId, cancellationToken);
        if (program is not null) return program;

        program = LoyaltyProgram.CreateDefault(query.AgencyId);
        await programRepository.AddAsync(program, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return program;
    }

    public async Task<IReadOnlyList<LoyaltyTier>> Handle(GetLoyaltyTiersQuery query, CancellationToken cancellationToken)
    {
        var tiers = await tierRepository.FindByAgencyIdAsync(query.AgencyId, cancellationToken);
        if (tiers.Count > 0) return tiers;

        var defaults = LoyaltyTier.CreateDefaults(query.AgencyId).ToList();
        foreach (var tier in defaults)
            await tierRepository.AddAsync(tier, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return defaults;
    }
}
