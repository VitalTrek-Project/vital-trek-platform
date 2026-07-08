using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.OutboundServices;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Payments.Stripe.Configuration;
using Microsoft.Extensions.Options;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Application.Internal.CommandServices;

/**
 * <summary>
 *     The subscription command service. Talks to the payment gateway through
 *     <see cref="IPaymentGatewayService" /> and persists via <see cref="ISubscriptionRepository" />.
 * </summary>
 */
public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentGatewayService paymentGatewayService,
    IUnitOfWork unitOfWork,
    IOptions<StripeSettings> stripeSettings,
    IStringLocalizer<ErrorMessages> localizer)
    : ISubscriptionCommandService
{
    public async Task<Result<string>> Handle(CreateCheckoutSessionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var settings = stripeSettings.Value;
            var session = await paymentGatewayService.CreateCheckoutSessionAsync(
                command.UserId, command.Plan, settings.SuccessUrl, settings.CancelUrl, cancellationToken);

            var subscription = new Subscription(command.UserId, command.Plan, session.SessionId, session.CustomerId);
            await subscriptionRepository.AddAsync(subscription, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<string>.Success(session.Url);
        }
        catch (PaymentGatewayNotConfiguredException ex)
        {
            return Result<string>.Failure(SubscriptionsError.StripeNotConfigured, ex.Message);
        }
        catch (OperationCanceledException)
        {
            return Result<string>.Failure(SubscriptionsError.OperationCancelled,
                localizer[nameof(SubscriptionsError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<string>.Failure(SubscriptionsError.DatabaseError,
                localizer[nameof(SubscriptionsError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<string>.Failure(SubscriptionsError.StripeError,
                "Could not start the Stripe checkout session.");
        }
    }

    public async Task<Result> Handle(ActivateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByStripeSessionIdAsync(command.StripeCheckoutSessionId, cancellationToken);
        if (subscription is null)
            return Result.Failure(SubscriptionsError.SubscriptionNotFound, "No subscription matches this Stripe session.");

        subscription.Activate(command.StripeSubscriptionId, command.StartDate, command.EndDate);
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> Handle(MarkPaymentFailedCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByStripeSessionIdAsync(command.StripeCheckoutSessionId, cancellationToken);
        if (subscription is null)
            return Result.Failure(SubscriptionsError.SubscriptionNotFound, "No subscription matches this Stripe session.");

        subscription.MarkPaymentFailed();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (subscription is null || subscription.Status != SubscriptionStatus.Active)
            return Result.Failure(SubscriptionsError.SubscriptionNotFound, "No active subscription to cancel.");

        if (subscription.StripeSubscriptionId is not null)
            await paymentGatewayService.CancelSubscriptionAsync(subscription.StripeSubscriptionId, cancellationToken);

        subscription.Cancel();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}
