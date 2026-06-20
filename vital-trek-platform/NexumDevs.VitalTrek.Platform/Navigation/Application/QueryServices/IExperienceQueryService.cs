using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;

public interface IExperienceQueryService
{
    Task<Experience?> Handle(GetExperienceByIdQuery query, CancellationToken cancellationToken);
    
    Task<IEnumerable<Experience>> Handle(GetAllExperiencesQuery query, CancellationToken cancellationToken);
}
