namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record UpdateEmergencyContactCommand(Guid UserId, Guid ContactId, string Name, string Relationship, string PhoneNumber);
