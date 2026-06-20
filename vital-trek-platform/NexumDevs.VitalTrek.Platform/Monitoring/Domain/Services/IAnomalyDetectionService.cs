using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Domain.Services;

public interface IAnomalyDetectionService
{
    Task DetectRouteDeviationAsync(LocationReading reading, CancellationToken cancellationToken);
    Task DetectCommunicationLossAsync(int touristId, DateTimeOffset lastReading, CancellationToken cancellationToken);
    Task DetectVitalAnomalyAsync(VitalSignReading reading, CancellationToken cancellationToken);
}