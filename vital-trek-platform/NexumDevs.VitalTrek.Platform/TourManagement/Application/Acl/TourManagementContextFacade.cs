using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Interfaces.Acl;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.Acl;

public class TourManagementContextFacade(ITourAssignmentRepository tourAssignmentRepository, ITourRepository tourRepository)
    : ITourManagementContextFacade
{
    public async Task<bool> IsTouristAssignedToAgencyAsync(Guid touristId, Guid agencyId, CancellationToken cancellationToken)
    {
        var assignments = await tourAssignmentRepository.FindByTouristIdAsync(touristId, cancellationToken);
        foreach (var assignment in assignments)
        {
            var tour = await tourRepository.FindByIdAsync(assignment.TourId, cancellationToken);
            if (tour is not null && tour.AgencyId == agencyId) return true;
        }

        return false;
    }
}
