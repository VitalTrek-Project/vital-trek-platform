using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.QueryServices;

public class ExperienceQueryService(IExperienceRepository experienceRepository) : IExperienceQueryService
{
    public async Task<Experience?> Handle(GetExperienceByIdQuery query, CancellationToken cancellationToken)
    {
        return await experienceRepository.FindByIdAsync(query.ExperienceId, cancellationToken);
    }
    
    public async Task<IEnumerable<Experience>> Handle(GetAllExperiencesQuery query, CancellationToken cancellationToken)
    {
        return await experienceRepository.ListAsync(cancellationToken);
    }
}