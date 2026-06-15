using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace NexumDevs.VitalTrek.Platform.Navigation.Interfaces.Rest;

public static class NavigationActionResultAssembler
{
    private static int ToStatusCodeFromNavigationError(NavigationError error)
    {
        return error switch
        {
            NavigationError.ExpeditionNotFound => StatusCodes.Status404NotFound,
            NavigationError.ExperienceNotFound => StatusCodes.Status404NotFound,
            NavigationError.ProgressNotLoaded => StatusCodes.Status404NotFound,
            NavigationError.WeatherNotLoaded => StatusCodes.Status404NotFound,

            NavigationError.OperationCancelled => StatusCodes.Status409Conflict,
            NavigationError.DatabaseError => StatusCodes.Status500InternalServerError,
            NavigationError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromCreateExpeditionResult(
        ControllerBase controller,
        Result<Expedition> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Expedition, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromNavigationError((NavigationError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetExpeditionResult(
        ControllerBase controller,
        Expedition? expedition,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Expedition, IActionResult> successAction)
    {
        if (expedition is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromNavigationError(NavigationError.ExpeditionNotFound),
                NavigationError.ExpeditionNotFound,
                errorLocalizer[nameof(NavigationError.ExpeditionNotFound)]
                );
        return successAction(expedition);
    }
}
