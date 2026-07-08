using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Iam.Domain.Repositories;

/**
 * <summary>
 *     The user repository
 * </summary>
 * <remarks>
 *     This repository is used to manage users
 * </remarks>
 */
public interface IUserRepository : IBaseRepository<User>
{
    /**
     * <summary>
     *     Find a user by id
     * </summary>
     * <param name="username">The username to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken);

    /**
     * <summary>
     *     Check if a user exists by username
     * </summary>
     * <param name="username">The username to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>True if the user exists, false otherwise</returns>
     */
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);

    /**
     * <summary>
     *     Find a user by its id
     * </summary>
     * <param name="id">The user id to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
}