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
    
    //Expedition
    
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

    public static IActionResult ToActionResultFromGetExpeditionByIdResult(
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
    
    //Experience
    
     public static IActionResult ToActionResultFromCreateExperienceResult(
        ControllerBase controller,
        Result<Experience> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Experience, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromNavigationError((NavigationError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
     
    public static IActionResult ToActionResultFromGetExperienceByIdResult(
        ControllerBase controller,
        Experience? experience,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Experience, IActionResult> successAction)
    {
        if (experience is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromNavigationError(NavigationError.ExperienceNotFound),
                NavigationError.ExperienceNotFound,
                errorLocalizer[nameof(NavigationError.ExperienceNotFound)]
                );
        return successAction(experience);
    }
    
    //Progress
    
     public static IActionResult ToActionResultFromCreateProgressResult(
        ControllerBase controller,
        Result<Progress> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Progress, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromNavigationError((NavigationError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
    
     public static IActionResult ToActionResultFromGetProgressByIdResult(
        ControllerBase controller,
        Progress? progress,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Progress, IActionResult> successAction)
    {
        if (progress is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromNavigationError(NavigationError.ProgressNotLoaded),
                NavigationError.ProgressNotLoaded,
                errorLocalizer[nameof(NavigationError.ProgressNotLoaded)]
                );
        return successAction(progress);
    }
     
     //Weather
     
      public static IActionResult ToActionResultFromCreateWeatherResult(
        ControllerBase controller,
        Result<Weather> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Weather, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        
        var statusCode = ToStatusCodeFromNavigationError((NavigationError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
    
     public static IActionResult ToActionResultFromGetWeatherByIdResult(
        ControllerBase controller,
        Weather? weather,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Weather, IActionResult> successAction)
    {
        if (weather is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromNavigationError(NavigationError.WeatherNotLoaded),
                NavigationError.WeatherNotLoaded,
                errorLocalizer[nameof(NavigationError.WeatherNotLoaded)]
                );
        return successAction(weather);
    }
}
