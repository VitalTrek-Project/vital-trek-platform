namespace NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.OutboundServices;

/**
 * <summary>
 *     Thrown by <see cref="IPaymentGatewayService" /> implementations when required
 *     provider credentials (e.g. Stripe:SecretKey) are missing from configuration.
 * </summary>
 */
public class PaymentGatewayNotConfiguredException(string message) : Exception(message);
