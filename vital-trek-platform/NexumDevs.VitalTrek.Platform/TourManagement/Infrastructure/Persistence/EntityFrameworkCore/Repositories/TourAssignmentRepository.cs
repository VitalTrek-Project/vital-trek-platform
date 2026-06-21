using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="ITourAssignmentRepository"/>.
/// Provides data access operations for <see cref="TourAssignment"/> entities.
/// </summary>
public class TourAssignmentRepository : BaseRepository<TourAssignment>, ITourAssignmentRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TourAssignmentRepository"/> class.
    /// </summary>
    /// <param name="context">
    /// The application database context used to access persistence storage.
    /// </param>
    public TourAssignmentRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a tour assignment by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the assignment.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The matching <see cref="TourAssignment"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<TourAssignment?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<TourAssignment>()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all assignments associated with a specific tour.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="TourAssignment"/> entities associated with the specified tour.
    /// </returns>
    public async Task<IEnumerable<TourAssignment>> FindByTourIdAsync(Guid tourId, CancellationToken cancellationToken)
    {
        return await Context.Set<TourAssignment>()
            .Where(a => a.TourId == tourId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all assignments associated with a specific tourist.
    /// </summary>
    /// <param name="touristId">
    /// The unique identifier of the tourist.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="TourAssignment"/> entities associated with the specified tourist.
    /// </returns>
    public async Task<IEnumerable<TourAssignment>> FindByTouristIdAsync(Guid touristId, CancellationToken cancellationToken)
    {
        return await Context.Set<TourAssignment>()
            .Where(a => a.TouristId == touristId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Determines whether an active assignment exists between
    /// the specified tour and tourist.
    /// </summary>
    /// <param name="tourId">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="touristId">
    /// The unique identifier of the tourist.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// <c>true</c> if an active assignment exists; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> ExistsAsync(Guid tourId, Guid touristId, CancellationToken cancellationToken)
    {
        return await Context.Set<TourAssignment>()
            .AnyAsync(a => a.TourId == tourId
                           && a.TouristId == touristId
                           && a.Status != EAssignmentStatus.Cancelled,
                cancellationToken);
    }
}