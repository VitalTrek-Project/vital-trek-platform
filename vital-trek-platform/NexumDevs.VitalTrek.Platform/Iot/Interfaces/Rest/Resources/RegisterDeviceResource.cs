namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

public record RegisterDeviceResource(
    string Name,
    string Type,
    string Status,
    int? ExpeditionId,
    int? TouristId
);