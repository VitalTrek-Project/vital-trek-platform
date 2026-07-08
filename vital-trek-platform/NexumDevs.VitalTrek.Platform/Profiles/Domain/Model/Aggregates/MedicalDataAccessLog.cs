namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
/// Immutable audit record of an agency staff member reading a tourist's medical/emergency
/// data. Its own append-only aggregate (not a child collection on TouristProfile), the same
/// way Loyalty keeps PointsTransaction independent of GamificationProfile to avoid loading
/// a growing audit trail every time the profile itself is read.
/// </summary>
public class MedicalDataAccessLog
{
    protected MedicalDataAccessLog()
    {
    }

    public MedicalDataAccessLog(Guid touristProfileId, Guid accessedByStaffUserId)
    {
        Id = Guid.NewGuid();
        TouristProfileId = touristProfileId;
        AccessedByStaffUserId = accessedByStaffUserId;
        AccessedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid TouristProfileId { get; private set; }
    public Guid AccessedByStaffUserId { get; private set; }
    public DateTimeOffset AccessedAt { get; private set; }
}
