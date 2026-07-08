using NexumDevs.VitalTrek.Platform.Iam.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Iam.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Iam.Interfaces.Acl;

namespace NexumDevs.VitalTrek.Platform.Iam.Application.Acl;

public class IamContextFacade(IUserCommandService userCommandService, IUserQueryService userQueryService)
    : IIamContextFacade
{
    public async Task<Guid?> CreateUser(string username, string password, UserRole role,
        CancellationToken cancellationToken)
    {
        var signUpCommand = new SignUpCommand(username, password, role);
        var signUpResult = await userCommandService.Handle(signUpCommand, cancellationToken);
        if (signUpResult.IsFailure) return null;
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery, cancellationToken);
        return result?.Id;
    }

    public async Task<Guid?> FetchUserIdByUsername(string username, CancellationToken cancellationToken)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery, cancellationToken);
        return result?.Id;
    }

    public async Task<string> FetchUsernameByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery, cancellationToken);
        return result?.Username ?? string.Empty;
    }
}
