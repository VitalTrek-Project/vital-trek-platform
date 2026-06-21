using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.QueryServices;

public class ProgressQueryService(IProgressRepository progressRepository) : IProgressQueryService
{
    public async Task<Progress?> Handle(GetProgressByIdQuery query, CancellationToken cancellationToken)
    {
        return await progressRepository.FindByIdAsync(query.ProgressId, cancellationToken);
    }
}
