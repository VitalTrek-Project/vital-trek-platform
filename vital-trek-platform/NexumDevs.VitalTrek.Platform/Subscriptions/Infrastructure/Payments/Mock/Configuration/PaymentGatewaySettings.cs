namespace NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Mock.Configuration;

/**
 * <summary>
 *     Bound from the "Payments" configuration section. Only redirect targets are needed —
 *     the mock gateway has no external credentials.
 * </summary>
 */
public class PaymentGatewaySettings
{
    public string SuccessUrl { get; set; } = "https://example.com/subscription/success";
    public string CancelUrl { get; set; } = "https://example.com/subscription/cancel";
}
