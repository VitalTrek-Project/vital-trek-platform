using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;

public interface ITouristProfileQueryService
{
    Task<TouristProfile?> Handle(GetTouristProfileByUserIdQuery query, CancellationToken cancellationToken);
    Task<ProfileCompletenessResult> Handle(GetTouristProfileCompletenessQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Authorized detail read by agency staff. Throws <see cref="Domain.Model.ProfilesError"/>
    /// with <see cref="Domain.Model.Errors.ProfilesErrors.UnauthorizedProfileAccess"/> when the
    /// tourist has no booking with the requesting agency. On success, appends a
    /// <see cref="Domain.Model.Aggregates.MedicalDataAccessLog"/> entry.
    /// </summary>
    Task<TouristProfile?> Handle(GetTouristProfileForAgencyQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<Domain.Model.Aggregates.MedicalDataAccessLog>> Handle(GetMedicalDataAccessLogQuery query, CancellationToken cancellationToken);
}
