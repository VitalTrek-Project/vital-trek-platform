using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Interfaces.Rest.Transform;
             //MonitoringActionResultAssembler
public static class MonitoringActionResultAssembler
{ 
     // --- Helper for transforming MonitoringError to StatusCode ---
    private static int ToStatusCodeFromMonitoringError(MonitoringError error)
    {
        return error switch
        {
           
            MonitoringError.IncidentNotFound => StatusCodes.Status404NotFound,
           
            MonitoringError.OperationCancelled => StatusCodes.Status409Conflict,
            MonitoringError.DatabaseError => StatusCodes.Status500InternalServerError,
            MonitoringError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest // Default
        };
    }

    // --- Specific Assembler Methods ---

  

    

    public static IActionResult ToActionResultFromCreateIncidentResult(
        ControllerBase controller,
        Result<Incident> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Incident, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMonitoringError((MonitoringError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    

    public static IActionResult ToActionResultFromGetIncidentByIdResult(
        ControllerBase controller,
        Incident? incident, // Direct incident object, not a Result
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Incident, IActionResult> successAction)
    {
        if (incident is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromMonitoringError(MonitoringError.IncidentNotFound),
                MonitoringError.IncidentNotFound,
                errorLocalizer[nameof(MonitoringError.IncidentNotFound)]
            );
        return successAction(incident);
    }


}