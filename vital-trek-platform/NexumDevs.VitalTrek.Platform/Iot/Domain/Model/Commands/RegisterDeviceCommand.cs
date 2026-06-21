namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;

public record RegisterDeviceCommand(
    string Name,
    string Type,
    string Status,
    int? ExpeditionId,
    int? TouristId
);