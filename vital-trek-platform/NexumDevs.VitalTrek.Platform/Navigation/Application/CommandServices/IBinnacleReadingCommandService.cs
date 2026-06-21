using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;

public interface IBinnacleReadingCommandService
{
    //Binnacle
    Task<BinnacleReading> Handle(RecordBinnacleReadingCommand command, CancellationToken cancellationToken);
}
