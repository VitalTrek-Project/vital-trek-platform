using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Application.CommandServices;

/**
 * <summary>
 *     The subscription command service
 * </summary>
 */
public interface ISubscriptionCommandService
{
    /// <summary>Starts a Stripe Checkout session and returns its hosted URL.</summary>
    Task<Result<string>> Handle(CreateCheckoutSessionCommand command, CancellationToken cancellationToken);

    Task<Result> Handle(ActivateSubscriptionCommand command, CancellationToken cancellationToken);

    Task<Result> Handle(MarkPaymentFailedCommand command, CancellationToken cancellationToken);

    Task<Result> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken);
}
