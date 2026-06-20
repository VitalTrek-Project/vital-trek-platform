using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;

public interface IVitalSignReadingCommandService
{
    Task<Result<VitalSignReading>> Handle(RecordVitalSignsCommand command, CancellationToken cancellationToken);
}