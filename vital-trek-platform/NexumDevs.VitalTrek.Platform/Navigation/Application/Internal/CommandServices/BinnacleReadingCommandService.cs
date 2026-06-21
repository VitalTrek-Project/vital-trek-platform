using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.CommandServices;

public class BinnacleReadingCommandService(
    IBinnacleReadingRepository binnacleReadingRepository,
    IUnitOfWork unitOfWork)
    : IBinnacleReadingCommandService
{
    public async Task<BinnacleReading> Handle(
        RecordBinnacleReadingCommand command,
        CancellationToken cancellationToken)
    {
        var reading = new BinnacleReading
        {
            ExpeditionId = command.ExpeditionId,
            TouristId = command.TouristId,
            Note = command.Note,
            MediaUrl = command.MediaUrl,
            CreatedAt = command.CreatedAt
        };

        await binnacleReadingRepository.AddAsync(
            reading,
            cancellationToken);

        await unitOfWork.CompleteAsync(
            cancellationToken);

        return reading;
    }
}
