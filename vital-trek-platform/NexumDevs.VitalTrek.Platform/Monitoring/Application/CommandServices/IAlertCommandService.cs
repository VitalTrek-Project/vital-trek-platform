using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;

public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(RaiseAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(DismissAlertCommand command, CancellationToken cancellationToken);
}