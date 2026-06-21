using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;

public interface IExpeditionCommandService
{
    Task<Result<Expedition>> Handle(CreateExpeditionCommand command, CancellationToken cancellationToken);
}
