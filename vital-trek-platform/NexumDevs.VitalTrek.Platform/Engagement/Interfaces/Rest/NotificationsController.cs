using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest;

/// <summary>
/// Simple in-app notification feed (points earned, tier up, badge earned, redemption
/// ready). Lives under its own top-level route since it isn't loyalty-specific by
/// nature, even though Engagement is its only producer today.
/// </summary>
[ApiController]
[Route("api/v1/notifications")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Notification endpoints")]
public class NotificationsController(
    INotificationCommandService commandService,
    INotificationQueryService queryService,
    IStringLocalizer<EngagementMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get a tourist's notifications", OperationId = "GetNotifications")]
    [SwaggerResponse(StatusCodes.Status200OK, "The notifications were found", typeof(IEnumerable<NotificationResource>))]
    public async Task<IActionResult> GetNotifications([FromQuery] Guid touristId, [FromQuery] bool? unreadOnly, CancellationToken cancellationToken)
    {
        var notifications = await queryService.Handle(new GetNotificationsQuery(touristId, unreadOnly), cancellationToken);
        return Ok(notifications.Select(NotificationResourceAssembler.ToResourceFromEntity));
    }

    [HttpPatch("{notificationId:guid}")]
    [SwaggerOperation(Summary = "Mark a notification as read", OperationId = "MarkNotificationRead")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The notification was updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The notification was not found")]
    public async Task<IActionResult> MarkRead([FromRoute] Guid notificationId, CancellationToken cancellationToken)
    {
        try
        {
            await commandService.Handle(new MarkNotificationReadCommand(notificationId), cancellationToken);
            return NoContent();
        }
        catch (EngagementError error)
        {
            return problemDetailsFactory.CreateProblemDetails(this, EngagementActionResultAssembler.ResolveStatusCode(error), (Enum?)null, localizer[error.ErrorCode]);
        }
    }
}
