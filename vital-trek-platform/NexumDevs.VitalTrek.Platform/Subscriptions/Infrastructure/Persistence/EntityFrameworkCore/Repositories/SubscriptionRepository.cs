using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/**
 * <summary>
 *     The subscription repository
 * </summary>
 */
public class SubscriptionRepository(AppDbContext context) : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<Subscription>()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Subscription?> FindByStripeSessionIdAsync(string stripeCheckoutSessionId, CancellationToken cancellationToken)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeCheckoutSessionId == stripeCheckoutSessionId, cancellationToken);
    }
}
