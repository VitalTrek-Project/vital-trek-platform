using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Pipeline.Filters;

/// <summary>
///     Global authorization filter that enforces tenant ownership for routes containing an
///     <c>agencyId</c> and/or <c>touristId</c> route segment: an authenticated Agency user may only
///     act on their own agency's data, and an authenticated Tourist user may only act on their own
///     data. Applies automatically to every controller (registered once in Program.cs), so individual
///     bounded contexts do not need to duplicate this check.
/// </summary>
public class TenantOwnershipFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated != true)
            return; // Let [Authorize]/the fallback policy handle unauthenticated requests.

        var role = user.FindFirstValue(ClaimTypes.Role);

        if (role == nameof(UserRole.Agency))
        {
            var claimAgencyId = user.FindFirstValue("agency_id");
            if (context.RouteData.Values.TryGetValue("agencyId", out var routeAgencyId) &&
                routeAgencyId is not null &&
                !string.Equals(routeAgencyId.ToString(), claimAgencyId, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
            }
        }
        else if (role == nameof(UserRole.Tourist))
        {
            var claimUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (context.RouteData.Values.TryGetValue("touristId", out var routeTouristId) &&
                routeTouristId is not null &&
                !string.Equals(routeTouristId.ToString(), claimUserId, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
