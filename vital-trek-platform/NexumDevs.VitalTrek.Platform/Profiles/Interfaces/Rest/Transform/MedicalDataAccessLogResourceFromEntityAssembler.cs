using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Transform;

public static class MedicalDataAccessLogResourceFromEntityAssembler
{
    public static MedicalDataAccessLogResource ToResourceFromEntity(MedicalDataAccessLog log) => new(
        log.Id, log.AccessedByStaffUserId, log.AccessedAt);
}
