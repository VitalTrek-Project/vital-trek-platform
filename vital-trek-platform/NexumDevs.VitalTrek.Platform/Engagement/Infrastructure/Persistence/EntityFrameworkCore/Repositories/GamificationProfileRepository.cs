using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IGamificationProfileRepository"/>.
/// Provides persistence operations for <see cref="GamificationProfile"/> aggregates.
/// </summary>
public class GamificationProfileRepository : BaseRepository<GamificationProfile>, IGamificationProfileRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GamificationProfileRepository"/> class.
    /// </summary>
    /// <param name="context">
    /// The application database context used to access persistence storage.
    /// </param>
    public GamificationProfileRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves the gamification profile associated with a specific tourist,
    /// including all awarded expeditions.
    /// </summary>
    /// <param name="touristId">The identifier of the tourist.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>
    /// The matching <see cref="GamificationProfile"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<GamificationProfile?> FindByTouristIdAsync(Guid touristId, CancellationToken cancellationToken)
    {
        return await Context.Set<GamificationProfile>()
            .Include(p => p.AwardedExpeditions)
            .FirstOrDefaultAsync(p => p.TouristId == touristId, cancellationToken);
    }
}
