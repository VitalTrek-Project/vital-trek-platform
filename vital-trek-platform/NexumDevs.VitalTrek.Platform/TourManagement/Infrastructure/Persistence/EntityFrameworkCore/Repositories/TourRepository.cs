using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TourRepository : BaseRepository<Tour>, ITourRepository
{
    public TourRepository(AppDbContext context) : base(context) { }

    public async Task<Tour?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .Include(t => t.Checkpoints)
            .Include(t => t.Assignments)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Tour>> FindByAgencyIdAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .Where(t => t.AgencyId == agencyId)
            .Include(t => t.Checkpoints)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tour>> FindByStatusAsync(ETourStatus status, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .Where(t => t.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tour>> SearchAsync(string term, CancellationToken cancellationToken)
    {
        var normalizedTerm = term.Trim().ToLower();

        return await Context.Set<Tour>()
            .Where(t => t.Title.ToLower().Contains(normalizedTerm)
                     || t.Description.ToLower().Contains(normalizedTerm))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(string title, Guid agencyId, CancellationToken cancellationToken)
    {
        return await Context.Set<Tour>()
            .AnyAsync(t => t.Title == title && t.AgencyId == agencyId, cancellationToken);
    }
}