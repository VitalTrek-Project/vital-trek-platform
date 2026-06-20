using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;

public interface IIncidentCommandService
{
    
    Task<Result<Incident>> Handle(CreateIncidentCommand command, CancellationToken cancellationToken);
}