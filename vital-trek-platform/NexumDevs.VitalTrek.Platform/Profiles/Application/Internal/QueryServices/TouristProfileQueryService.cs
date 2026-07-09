using NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Acl;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Internal.QueryServices;

public class TouristProfileQueryService(
    ITouristProfileRepository repository,
    IMedicalDataAccessLogRepository accessLogRepository,
    ITourManagementContextFacade tourManagementContextFacade,
    IUnitOfWork unitOfWork)
    : ITouristProfileQueryService
{
    public async Task<TouristProfile?> Handle(GetTouristProfileByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.FindByUserIdAsync(query.UserId, cancellationToken);
    }

    public async Task<ProfileCompletenessResult> Handle(GetTouristProfileCompletenessQuery query, CancellationToken cancellationToken)
    {
        var profile = await repository.FindByUserIdAsync(query.UserId, cancellationToken);
        return profile?.EvaluateCompleteness() ?? new ProfileCompletenessResult(false, 0, ["profile"]);
    }

    public async Task<TouristProfile?> Handle(GetTouristProfileForAgencyQuery query, CancellationToken cancellationToken)
    {
        var profile = await repository.FindByUserIdAsync(query.TouristUserId, cancellationToken);
        if (profile is null) return null;

        var isAuthorized = await tourManagementContextFacade.IsTouristAssignedToAgencyAsync(
            query.TouristUserId, query.AgencyId, cancellationToken);
        if (!isAuthorized)
            throw new ProfilesError(ProfilesErrors.UnauthorizedProfileAccess,
                "This tourist has no booking with your agency.");

        await accessLogRepository.AddAsync(new MedicalDataAccessLog(profile.Id, query.RequestedByStaffUserId), cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return profile;
    }

    public async Task<IReadOnlyList<MedicalDataAccessLog>> Handle(GetMedicalDataAccessLogQuery query, CancellationToken cancellationToken)
    {
        var profile = await repository.FindByUserIdAsync(query.TouristUserId, cancellationToken)
                      ?? throw new ProfilesError(ProfilesErrors.ProfileNotFound, "Tourist profile not found.");

        var isAuthorized = await tourManagementContextFacade.IsTouristAssignedToAgencyAsync(
            query.TouristUserId, query.AgencyId, cancellationToken);
        if (!isAuthorized)
            throw new ProfilesError(ProfilesErrors.UnauthorizedProfileAccess,
                "This tourist has no booking with your agency.");

        return await accessLogRepository.FindByTouristProfileIdAsync(profile.Id, cancellationToken);
    }
}
