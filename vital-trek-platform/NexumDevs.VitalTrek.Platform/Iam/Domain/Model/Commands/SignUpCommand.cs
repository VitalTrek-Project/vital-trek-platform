using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Commands;

/**
 * <summary>
 *     The sign up command
 * </summary>
 * <remarks>
 *     This command object includes the username, password and role to sign up
 * </remarks>
 */
public record SignUpCommand(string Username, string Password, UserRole Role);
