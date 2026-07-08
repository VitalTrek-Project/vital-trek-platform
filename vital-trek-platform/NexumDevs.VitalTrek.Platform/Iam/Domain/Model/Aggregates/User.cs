using System.Text.Json.Serialization;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Aggregates;

/**
 * <summary>
 *     The user aggregate
 * </summary>
 * <remarks>
 *     This class is used to represent a user
 * </remarks>
 */
public partial class User(string username, string passwordHash, UserRole role)
{
    public User() : this(string.Empty, string.Empty, UserRole.Tourist)
    {
    }

    public Guid Id { get; private init; } = Guid.NewGuid();
    public string Username { get; private set; } = username;
    public UserRole Role { get; private init; } = role;

    /**
     * <summary>
     *     The agency this user belongs to. Only set when <see cref="Role" /> is <see cref="UserRole.Agency" />.
     * </summary>
     */
    public Guid? AgencyId { get; private init; } = role == UserRole.Agency ? Guid.NewGuid() : null;

    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;

    /**
     * <summary>
     *     Update the username
     * </summary>
     * <param name="username">The new username</param>
     * <returns>The updated user</returns>
     */
    public User UpdateUsername(string username)
    {
        Username = username;
        return this;
    }

    /**
     * <summary>
     *     Update the password hash
     * </summary>
     * <param name="passwordHash">The new password hash</param>
     * <returns>The updated user</returns>
     */
    public User UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }
}
