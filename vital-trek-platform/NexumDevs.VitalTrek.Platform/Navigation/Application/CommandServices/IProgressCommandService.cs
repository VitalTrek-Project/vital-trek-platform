using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;

public interface IProgressCommandService
{
    Task<Result<Progress>> Handle(CreateProgressCommand command, CancellationToken cancellationToken);
}
