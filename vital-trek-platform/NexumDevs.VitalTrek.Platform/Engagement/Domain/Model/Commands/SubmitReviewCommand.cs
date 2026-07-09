namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

public record SubmitReviewCommand(Guid AgencyId, Guid TouristId, int ExpeditionId, int Rating, string? Comment);
