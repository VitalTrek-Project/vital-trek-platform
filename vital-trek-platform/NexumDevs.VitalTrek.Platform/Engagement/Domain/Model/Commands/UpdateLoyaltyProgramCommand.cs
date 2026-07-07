namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record UpdateLoyaltyProgramCommand(
    Guid AgencyId,
    int PointsPerExpeditionCompleted,
    int PointsPerExpeditionBooked,
    int PointsPerReferral,
    int PointsPerReview,
    int? ExpirationMonths);
