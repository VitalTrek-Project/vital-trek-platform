using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;

public interface IExpeditionQueryService
{
    Task<Expedition?> Handle(GetExpeditionByIdQuery query, CancellationToken cancellationToken);
    
    Task<IEnumerable<Expedition>> Handle(GetAllExpeditionsQuery query, CancellationToken cancellationToken);
}
