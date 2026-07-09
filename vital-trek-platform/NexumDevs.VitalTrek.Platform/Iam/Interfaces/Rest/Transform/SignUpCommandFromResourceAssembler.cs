using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Iam.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler responsible for transforming a <see cref="SignUpResource" /> into a <see cref="SignUpCommand" />.
/// </summary>
public static class SignUpCommandFromResourceAssembler
{
    /// <summary>
    ///     Converts a <see cref="SignUpResource" /> to a <see cref="SignUpCommand" />.
    /// </summary>
    /// <param name="resource">
    ///     The <see cref="SignUpResource" /> containing the sign-up data. Must not be null.
    /// </param>
    /// <returns>
    ///     A new <see cref="SignUpCommand" /> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the input <paramref name="resource" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="resource" />'s role is not a valid role.</exception>
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "SignUpResource cannot be null when converting to command.");
        if (!Enum.TryParse<UserRole>(resource.Role, true, out var role))
            throw new ArgumentException($"'{resource.Role}' is not a valid role. Expected 'Tourist' or 'Agency'.",
                nameof(resource));
        return new SignUpCommand(resource.Username, resource.Password, role);
    }
}
