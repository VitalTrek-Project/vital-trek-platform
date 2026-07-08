namespace NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Stripe.Configuration;

/**
 * <summary>
 *     Bound from the "Stripe" configuration section (appsettings / Railway env vars
 *     Stripe__SecretKey, Stripe__WebhookSecret, Stripe__SuccessUrl, Stripe__CancelUrl).
 *     Intentionally NOT validated at startup: the app must boot and the rest of the
 *     platform must keep working even before Stripe credentials exist. Validation happens
 *     lazily, the first time a checkout is attempted.
 * </summary>
 */
public class StripeSettings
{
    public string? SecretKey { get; set; }
    public string? WebhookSecret { get; set; }
    public string SuccessUrl { get; set; } = "https://example.com/subscription/success";
    public string CancelUrl { get; set; } = "https://example.com/subscription/cancel";
}
