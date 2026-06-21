using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;

public interface IProgressQueryService
{
    Task<Progress?> Handle(GetProgressByIdQuery query, CancellationToken cancellationToken);
}
