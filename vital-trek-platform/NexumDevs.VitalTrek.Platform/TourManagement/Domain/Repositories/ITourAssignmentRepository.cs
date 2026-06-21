using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

/// <summary>
/// Defines the repository contract for managing
/// <see cref="TourAssignment"/> entities.
/// </summary>
public interface ITourAssignmentRepository : IBaseRepository<TourAssignment>
{
    /// <summary>
    /// Retrieves a tour assignment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the assignment.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="TourAssignment"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<TourAssignment?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all assignments associated with a specific tour.
    /// </summary>
    /// <param name="tourId">The unique identifier of the tour.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="TourAssignment"/> instances associated with the specified tour.
    /// </returns>
    Task<IEnumerable<TourAssignment>> FindByTourIdAsync(Guid tourId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all assignments associated with a specific tourist.
    /// </summary>
    /// <param name="touristId">The unique identifier of the tourist.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// A collection of <see cref="TourAssignment"/> instances associated with the specified tourist.
    /// </returns>
    Task<IEnumerable<TourAssignment>> FindByTouristIdAsync(Guid touristId, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether an assignment already exists between
    /// a specific tour and tourist.
    /// </summary>
    /// <param name="tourId">The unique identifier of the tour.</param>
    /// <param name="touristId">The unique identifier of the tourist.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// <c>true</c> if the assignment exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ExistsAsync(Guid tourId, Guid touristId, CancellationToken cancellationToken);
}