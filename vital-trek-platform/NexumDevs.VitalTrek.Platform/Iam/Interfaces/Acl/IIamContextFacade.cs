using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<Guid?> CreateUser(string username, string password, UserRole role, CancellationToken cancellationToken);
    Task<Guid?> FetchUserIdByUsername(string username, CancellationToken cancellationToken);
    Task<string> FetchUsernameByUserId(Guid userId, CancellationToken cancellationToken);
}
