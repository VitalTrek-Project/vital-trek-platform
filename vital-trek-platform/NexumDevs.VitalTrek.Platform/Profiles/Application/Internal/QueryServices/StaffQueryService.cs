using NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Internal.QueryServices;

public class StaffQueryService(IStaffProfileRepository profileRepository, IStaffPreferencesRepository preferencesRepository)
    : IStaffQueryService
{
    public async Task<StaffProfile?> Handle(GetStaffProfileByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.FindByUserIdAsync(query.UserId, cancellationToken);
    }

    public async Task<StaffPreferences?> Handle(GetStaffPreferencesByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await preferencesRepository.FindByUserIdAsync(query.UserId, cancellationToken);
    }
}
