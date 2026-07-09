namespace NexumDevs.VitalTrek.Platform.Iam.Interfaces.Rest.Resources;

public record AuthenticatedUserResource(Guid Id, string Username, string Role, Guid? AgencyId, string Token);
