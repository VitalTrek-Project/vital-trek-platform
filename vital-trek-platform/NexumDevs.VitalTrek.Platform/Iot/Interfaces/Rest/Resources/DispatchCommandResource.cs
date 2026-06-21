namespace NexumDevs.VitalTrek.Platform.Iot.Interfaces.Rest.Resources;

public record DispatchCommandResource(string? LastCommand, DateTime? LastSeen);