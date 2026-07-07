namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record RedeemReferralCodeCommand(Guid AgencyId, string Code, Guid ReferredTouristId);
