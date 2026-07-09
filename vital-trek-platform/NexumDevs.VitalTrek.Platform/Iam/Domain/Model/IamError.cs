namespace NexumDevs.VitalTrek.Platform.Iam.Domain.Model;

public enum IamError
{
    None,
    UserNotFound,
    UsernameAlreadyTaken,
    InvalidCredentials,
    InvalidRole,
    OperationCancelled,
    DatabaseError,
    InternalServerError,
    ExternalServiceError
}