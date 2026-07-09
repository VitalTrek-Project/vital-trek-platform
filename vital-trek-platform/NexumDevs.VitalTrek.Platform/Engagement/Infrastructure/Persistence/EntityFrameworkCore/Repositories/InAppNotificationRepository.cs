using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class InAppNotificationRepository(AppDbContext context)
    : BaseRepository<InAppNotification>(context), IInAppNotificationRepository
{
    public async Task<IReadOnlyList<InAppNotification>> FindByRecipientAsync(Guid touristId, bool? unreadOnly, CancellationToken cancellationToken)
    {
        var query = Context.Set<InAppNotification>().Where(n => n.RecipientTouristId == touristId);
        if (unreadOnly == true) query = query.Where(n => !n.IsRead);
        return await query.OrderByDescending(n => n.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task<InAppNotification?> FindByIdAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        return await Context.Set<InAppNotification>()
            .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
    }
}
