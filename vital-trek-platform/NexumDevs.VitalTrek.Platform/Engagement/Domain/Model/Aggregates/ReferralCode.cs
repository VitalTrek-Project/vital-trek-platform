using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Services;

namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing a tourist's shareable referral code for one agency's program.
/// </summary>
public class ReferralCode
{
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected ReferralCode() { }

    public ReferralCode(Guid agencyId, Guid touristId)
    {
        Id = Guid.NewGuid();
        AgencyId = agencyId;
        TouristId = touristId;
        Code = CodeGenerator.Generate(6);
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid AgencyId { get; private set; }
    public Guid TouristId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
}
