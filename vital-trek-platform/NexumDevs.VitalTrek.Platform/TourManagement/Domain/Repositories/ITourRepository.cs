using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

/// <summary>
/// Defines the repository contract for managing
/// <see cref="Tour"/> aggregates.
/// </summary>
public interface ITourRepository : IBaseRepository<Tour>
{
    /// <summary>
    /// Retrieves a tour by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tour.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="Tour"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Tour?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tours belonging to a specific agency.
    /// </summary>
    /// <param name="agencyId">The unique identifier of the agency.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances associated with the specified agency.
    /// </returns>
    Task<IEnumerable<Tour>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tours with the specified status.
    /// </summary>
    /// <param name="status">The tour status to filter by.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances matching the specified status.
    /// </returns>
    Task<IEnumerable<Tour>> FindByStatusAsync(ETourStatus status, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for tours that match the specified search term.
    /// </summary>
    /// <param name="term">The search term used to find matching tours.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="Tour"/> instances matching the search criteria.
    /// </returns>
    Task<IEnumerable<Tour>> SearchAsync(string term, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether a tour with the specified title already exists
    /// within the given agency.
    /// </summary>
    /// <param name="title">The title of the tour.</param>
    /// <param name="agencyId">The unique identifier of the agency.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// <c>true</c> if a tour with the specified title exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ExistsByTitleAsync(string title, Guid agencyId, CancellationToken cancellationToken);
}