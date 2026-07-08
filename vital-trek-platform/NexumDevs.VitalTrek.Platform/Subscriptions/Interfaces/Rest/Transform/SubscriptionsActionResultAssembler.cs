using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Interfaces.Rest.Transform;

public static class SubscriptionsActionResultAssembler
{
    private static int ToStatusCode(SubscriptionsError error)
    {
        return error switch
        {
            SubscriptionsError.SubscriptionNotFound => StatusCodes.Status404NotFound,
            SubscriptionsError.AlreadyActive => StatusCodes.Status409Conflict,
            SubscriptionsError.InvalidPlan => StatusCodes.Status400BadRequest,
            SubscriptionsError.OperationCancelled => StatusCodes.Status409Conflict,
            SubscriptionsError.StripeNotConfigured => StatusCodes.Status503ServiceUnavailable,
            SubscriptionsError.StripeError => StatusCodes.Status502BadGateway,
            SubscriptionsError.DatabaseError => StatusCodes.Status500InternalServerError,
            SubscriptionsError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromResult<T>(
        ControllerBase controller,
        Result<T> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var error = (SubscriptionsError)result.Error!;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(error), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromResult(
        ControllerBase controller,
        Result result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction();

        var error = (SubscriptionsError)result.Error!;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(error), result.Error, result.Message);
    }
}
