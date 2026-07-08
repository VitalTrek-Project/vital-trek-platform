namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record AddEmergencyContactCommand(Guid UserId, string Name, string Relationship, string PhoneNumber);
