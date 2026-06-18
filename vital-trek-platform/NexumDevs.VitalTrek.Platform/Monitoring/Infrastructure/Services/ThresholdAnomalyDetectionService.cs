using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Services;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Services;

public class ThresholdAnomalyDetectionService(IAlertRepository alertRepository, IUnitOfWork unitOfWork) : IAnomalyDetectionService
{
    public Task DetectRouteDeviationAsync(LocationReading reading, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to detect route deviation
        return Task.CompletedTask;
    }

    public Task DetectCommunicationLossAsync(int touristId, DateTimeOffset lastReading, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to detect communication loss
        return Task.CompletedTask;
    }

    public async Task DetectVitalAnomalyAsync(VitalSignReading reading, CancellationToken cancellationToken)
    {
        // These values should come from configuration
        const int minHr = 60;
        const int maxHr = 100;
        const double minSpO2 = 95.0;
        const double maxTemp = 37.5;

        if (reading.IsCritical(minHr, maxHr, minSpO2, maxTemp))
        {
            var alert = new Alert(
                reading.ExpeditionId,
                reading.TouristId,
                AlertType.VITAL_SIGNS_CRITICAL,
                AlertSeverity.CRITICAL,
                "Critical vital signs detected for tourist."
            );
            await alertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}