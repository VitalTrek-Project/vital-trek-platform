using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

/// <summary>
/// Represents the request payload for awarding points to a gamification profile.
/// </summary>
/// <param name="ExpeditionId">
/// The unique identifier of the expedition for which points are being awarded.
/// </param>
/// <param name="Points">
/// The number of points to award. Must be greater than zero.
/// </param>
public record AwardPointsResource(
    [Required] Guid ExpeditionId,
    [Range(1, int.MaxValue, ErrorMessage = "Points must be greater than zero.")] int Points);
