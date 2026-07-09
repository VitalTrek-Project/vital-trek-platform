using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record RewardResource(Guid Id, string Name, string Description, int PointsCost, int? Stock, bool IsActive);

public record CreateRewardResource(
    [Required] string Name,
    string Description,
    [Range(1, int.MaxValue)] int PointsCost,
    [Range(0, int.MaxValue)] int? Stock);

public record UpdateRewardResource(
    [Required] string Name,
    string Description,
    [Range(1, int.MaxValue)] int PointsCost,
    [Range(0, int.MaxValue)] int? Stock,
    bool IsActive);
