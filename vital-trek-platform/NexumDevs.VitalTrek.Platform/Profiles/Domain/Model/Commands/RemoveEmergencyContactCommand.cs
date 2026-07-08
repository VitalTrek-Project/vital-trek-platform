namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record RemoveEmergencyContactCommand(Guid UserId, Guid ContactId);
