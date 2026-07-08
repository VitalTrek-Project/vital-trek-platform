using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;

public interface IStaffQueryService
{
    Task<StaffProfile?> Handle(GetStaffProfileByUserIdQuery query, CancellationToken cancellationToken);
    Task<StaffPreferences?> Handle(GetStaffPreferencesByUserIdQuery query, CancellationToken cancellationToken);
}
