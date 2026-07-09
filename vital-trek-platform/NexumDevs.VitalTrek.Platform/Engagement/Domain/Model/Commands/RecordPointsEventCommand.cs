using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

/// <summary>
/// Records that a points-earning event happened. The backend resolves how many
/// points it is worth from the agency's <c>LoyaltyProgram</c> configuration — the
/// caller never supplies a point amount. <paramref name="SourceId"/> together with
/// <paramref name="Type"/> is the idempotency key (the same source can't award twice).
/// </summary>
public record RecordPointsEventCommand(Guid AgencyId, Guid TouristId, PointsTransactionType Type, string? SourceId);
