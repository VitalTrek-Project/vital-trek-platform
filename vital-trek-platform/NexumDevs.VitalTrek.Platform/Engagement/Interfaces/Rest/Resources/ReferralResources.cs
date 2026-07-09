using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Engagement.Interfaces.Rest.Resources;

public record ReferralResource(Guid Id, string Status, DateTimeOffset CreatedAt, DateTimeOffset? CompletedAt);

public record RedeemReferralCodeResource([Required] string Code, [Required] Guid ReferredTouristId);
