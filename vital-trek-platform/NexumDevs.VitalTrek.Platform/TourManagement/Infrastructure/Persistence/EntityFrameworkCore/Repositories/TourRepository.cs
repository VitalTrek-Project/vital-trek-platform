using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="ITourRepository"/>.
/// Provides persistence operations for <see cref="Tour"/> aggregates.
/// </summary>
public class TourRepository : BaseRepository<Tour>, ITourRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TourRepository"/> class.
    /// </summary>
    /// <param name="context">
    /// The application database context used to access persistence storage.
    /// </param>
    public TourRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a tour by its unique identifier, including its checkpoints
    /// and tourist assignments.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the tour.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The matching <see cref="Tour"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Tour?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .Include(t => t.Checkpoints)
            .Include(t => t.Assignments)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all tours belonging to a specific agency.
    /// </summary>
    /// <param name="agencyId">
    /// The unique identifier of the agency.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Tour"/> aggregates associated with the specified agency.
    /// </returns>
    public async Task<IEnumerable<Tour>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .Where(t => t.AgencyId == agencyId)
            .Include(t => t.Checkpoints)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all tours that match the specified status.
    /// </summary>
    /// <param name="status">
    /// The tour status used as a filter.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Tour"/> aggregates matching the specified status.
    /// </returns>
    public async Task<IEnumerable<Tour>> FindByStatusAsync(ETourStatus status, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .Where(t => t.Status == status)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Searches for tours whose title or description contains
    /// the specified search term.
    /// </summary>
    /// <param name="term">
    /// The search term used to find matching tours.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="Tour"/> aggregates matching the search criteria.
    /// </returns>
    public async Task<IEnumerable<Tour>> SearchAsync(string term, CancellationToken cancellationToken)
    {
        var normalizedTerm = term.Trim().ToLower();

        return await Context.Set<Tour>()
            .Where(t => t.Title.ToLower().Contains(normalizedTerm)
                     || t.Description.ToLower().Contains(normalizedTerm))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Determines whether a tour with the specified title already exists
    /// within the given agency.
    /// </summary>
    /// <param name="title">
    /// The title of the tour.
    /// </param>
    /// <param name="agencyId">
    /// The unique identifier of the agency.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// <c>true</c> if a matching tour exists; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> ExistsByTitleAsync(string title, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .AnyAsync(t => t.Title == title && t.AgencyId == agencyId, cancellationToken);
    }
}