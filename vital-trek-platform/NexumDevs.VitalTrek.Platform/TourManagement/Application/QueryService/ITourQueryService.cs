using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Queries;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;

/// <summary>
/// Defines the contract for handling tour-related queries,
/// including retrieving tours, searching tours, and obtaining
/// tourist assignments associated with a tour.
/// </summary>
public interface ITourQueryService
{
    /// <summary>
    /// Retrieves a tour by its unique identifier.
    /// </summary>
    /// <param name="query">The query containing the tour identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="Tour"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Tour?> Handle(GetTourByIdQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tours belonging to a specific agency.
    /// </summary>
    /// <param name="query">The query containing the agency identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances associated with the specified agency.
    /// </returns>
    Task<IEnumerable<Tour>> Handle(GetAllToursByAgencyQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tourist assignments associated with a specific tour.
    /// </summary>
    /// <param name="query">The query containing the tour identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="TourAssignment"/> instances associated with the specified tour.
    /// </returns>
    Task<IEnumerable<TourAssignment>> Handle(GetAssignedTouristsByTourQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for tours that match the specified search criteria.
    /// </summary>
    /// <param name="query">The query containing the search term.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances matching the search criteria.
    /// </returns>
    Task<IEnumerable<Tour>> Handle(SearchToursQuery query, CancellationToken cancellationToken);
}