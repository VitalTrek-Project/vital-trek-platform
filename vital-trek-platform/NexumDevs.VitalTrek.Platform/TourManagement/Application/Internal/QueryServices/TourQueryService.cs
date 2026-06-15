using NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.QueryServices;

/// <summary>
/// Implementación de las consultas (queries) del BC TourManagement.
/// Equivalente a "CategoryQueryService" / "TutorialQueryService".
/// </summary>
public class TourQueryService : ITourQueryService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourAssignmentRepository _assignmentRepository;

    public TourQueryService(ITourRepository tourRepository, ITourAssignmentRepository assignmentRepository)
    {
        _tourRepository = tourRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<Tour?> Handle(GetTourByIdQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.FindByIdAsync(query.TourId, cancellationToken);
    }

    public async Task<IEnumerable<Tour>> Handle(GetAllToursByAgencyQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.FindByAgencyIdAsync(query.AgencyId, cancellationToken);
    }

    public async Task<IEnumerable<TourAssignment>> Handle(GetAssignedTouristsByTourQuery query, CancellationToken cancellationToken)
    {
        return await _assignmentRepository.FindByTourIdAsync(query.TourId, cancellationToken);
    }

    public async Task<IEnumerable<Tour>> Handle(SearchToursQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.SearchAsync(query.Term, cancellationToken);
    }
}