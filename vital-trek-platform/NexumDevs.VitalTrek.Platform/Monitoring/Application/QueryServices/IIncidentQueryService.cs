
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;

public interface IIncidentQueryService
{
 
    Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken);

    
    Task<IEnumerable<Incident>> Handle(GetAllIncidentsQuery query, CancellationToken cancellationToken);

  
    
}