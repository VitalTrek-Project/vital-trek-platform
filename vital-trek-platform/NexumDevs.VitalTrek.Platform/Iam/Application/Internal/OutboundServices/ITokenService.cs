using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Aggregates;

namespace NexumDevs.VitalTrek.Platform.Iam.Application.Internal.OutboundServices;

/**
 * <summary>
 *     The token service interface
 * </summary>
 * <remarks>
 *     This interface is used to generate JWT tokens. Token validation is delegated to ASP.NET Core's
 *     standard JWT Bearer authentication handler, configured in Program.cs.
 * </remarks>
 */
public interface ITokenService
{
    /**
     * <summary>
     *     Generate a JWT token
     * </summary>
     * <param name="user">The user to generate the token for</param>
     * <returns>The generated token</returns>
     */
    string GenerateToken(User user);
}
