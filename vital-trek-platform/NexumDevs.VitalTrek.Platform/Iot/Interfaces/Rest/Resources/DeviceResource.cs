namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

public record DeviceResource(
    int Id,
    string Name,
    string Type,
    string Status,
    DateTime? LastSeen,
    string? LastCommand,
    int? ExpeditionId,
    int? TouristId
);