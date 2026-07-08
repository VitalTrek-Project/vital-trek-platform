using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Subscriptions.Domain.Model.Aggregates;

namespace NexumDevs.VitalTrek.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/**
 * <summary>
 *     EF Core Fluent API configuration for the Subscriptions bounded context.
 *     Invoked from <c>AppDbContext.OnModelCreating</c>.
 * </summary>
 */
public static class ModelBuilderExtensions
{
    public static void ApplySubscriptionsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Subscriptions");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Plan)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(s => s.StripeCustomerId).HasMaxLength(100);
            entity.Property(s => s.StripeCheckoutSessionId).HasMaxLength(100);
            entity.Property(s => s.StripeSubscriptionId).HasMaxLength(100);

            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.StripeCheckoutSessionId).IsUnique();
        });
    }
}
