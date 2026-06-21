using NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.QueryServices;

/// <summary>
/// Query service responsible for handling tour-related read operations,
/// including retrieving tours, searching tours, and obtaining tour assignments.
/// </summary>
public class TourQueryService : ITourQueryService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourAssignmentRepository _assignmentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TourQueryService"/> class.
    /// </summary>
    /// <param name="tourRepository">Repository used to retrieve tour information.</param>
    /// <param name="assignmentRepository">Repository used to retrieve tour assignment information.</param>
    public TourQueryService(
        ITourRepository tourRepository,
        ITourAssignmentRepository assignmentRepository)
    {
        _tourRepository = tourRepository;
        _assignmentRepository = assignmentRepository;
    }

    /// <summary>
    /// Retrieves a tour by its unique identifier.
    /// </summary>
    /// <param name="query">The query containing the identifier of the tour to retrieve.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The <see cref="Tour"/> that matches the specified identifier;
    /// otherwise, <c>null</c> if no tour is found.
    /// </returns>
    public async Task<Tour?> Handle(GetTourByIdQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.FindByIdAsync(query.TourId, cancellationToken);
    }

    /// <summary>
    /// Retrieves all tours associated with a specific agency.
    /// </summary>
    /// <param name="query">The query containing the agency identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances belonging to the specified agency.
    /// </returns>
    public async Task<IEnumerable<Tour>> Handle(GetAllToursByAgencyQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.FindByAgencyIdAsync(query.AgencyId, cancellationToken);
    }

    /// <summary>
    /// Retrieves all tourist assignments for a specific tour.
    /// </summary>
    /// <param name="query">The query containing the tour identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="TourAssignment"/> entities associated with the specified tour.
    /// </returns>
    public async Task<IEnumerable<TourAssignment>> Handle(GetAssignedTouristsByTourQuery query, CancellationToken cancellationToken)
    {
        return await _assignmentRepository.FindByTourIdAsync(query.TourId, cancellationToken);
    }

    /// <summary>
    /// Searches for tours that match the specified search criteria.
    /// </summary>
    /// <param name="query">The query containing the search term.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances that match the specified search term.
    /// </returns>
    public async Task<IEnumerable<Tour>> Handle(SearchToursQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.SearchAsync(query.Term, cancellationToken);
    }
}