using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.QueryServices;

public class ExpeditionQueryService(IExpeditionRepository expeditionRepository) : IExpeditionQueryService
{
    public async Task<Expedition?> Handle(GetExpeditionByIdQuery query, CancellationToken cancellationToken)
    {
        return await expeditionRepository.FindByIdAsync(query.ExpeditionId, cancellationToken);
    }
    
    public async Task<IEnumerable<Expedition>> Handle(GetAllExpeditionsQuery query, CancellationToken cancellationToken)
    {
        return await expeditionRepository.ListAsync(cancellationToken);
    }
}
