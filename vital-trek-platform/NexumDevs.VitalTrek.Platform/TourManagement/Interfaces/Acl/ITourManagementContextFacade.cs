namespace NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Acl;

/// <summary>
/// Anti-corruption layer exposing read-only TourManagement facts to other bounded contexts,
/// without letting them query TourManagement's own tables directly. Mirrors
/// <c>Iam.Interfaces.Acl.IIamContextFacade</c>.
/// </summary>
public interface ITourManagementContextFacade
{
    /// <summary>
    /// Whether the given tourist has (or has had) an assignment on a tour owned by the
    /// given agency. Used by Profiles to authorize an agency staff member's read of a
    /// tourist's profile — only tourists booked with that agency's tours are visible to it.
    /// </summary>
    Task<bool> IsTouristAssignedToAgencyAsync(Guid touristId, Guid agencyId, CancellationToken cancellationToken);
}
