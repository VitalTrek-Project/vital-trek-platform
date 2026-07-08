namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;

/// <summary>
/// Authorized detail read of a tourist's full profile (including medical/emergency data) by
/// agency staff. Handling this query both checks that the tourist has a booking with the
/// requesting agency (via TourManagement's ACL facade) and appends a
/// <see cref="Aggregates.MedicalDataAccessLog"/> entry — reading medical data is itself an
/// auditable action.
/// </summary>
public record GetTouristProfileForAgencyQuery(Guid AgencyId, Guid TouristUserId, Guid RequestedByStaffUserId);
