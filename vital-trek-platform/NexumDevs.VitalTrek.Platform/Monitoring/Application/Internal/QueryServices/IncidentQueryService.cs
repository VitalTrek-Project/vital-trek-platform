using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.QueryServices;

public class IncidentQueryService (IIncidentRepository incidentRepository) : IIncidentQueryService
{
    public async Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken)
    {
        return await incidentRepository.FindByIdAsync(query.IncidentId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Incident>> Handle(GetAllIncidentsQuery query, CancellationToken cancellationToken)
    {
        return await incidentRepository.ListAsync(cancellationToken);
    }

  
}