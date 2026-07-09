namespace NexumDevs.VitalTrek.Platform.Iam.Interfaces.Rest.Resources;

public record UserResource(Guid Id, string Username, string Role, Guid? AgencyId);
