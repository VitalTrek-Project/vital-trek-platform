using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the Engagement bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies all Entity Framework Core configurations
    /// related to the Engagement bounded context.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ModelBuilder"/> used to configure entity mappings.
    /// </param>
    public static void ApplyEngagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<GamificationProfile>(entity =>
        {
            entity.ToTable("GamificationProfiles");

            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.TouristId)
                .IsUnique();

            entity.Property(p => p.TotalPoints)
                .IsRequired();

            entity.HasMany(p => p.AwardedExpeditions)
                .WithOne()
                .HasForeignKey(e => e.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Metadata
                .FindNavigation(nameof(GamificationProfile.AwardedExpeditions))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Ignore(p => p.Rank);
            entity.Ignore(p => p.UnlockedBadges);
        });

        builder.Entity<AwardedExpedition>(entity =>
        {
            entity.ToTable("AwardedExpeditions");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.ProfileId, e.ExpeditionId })
                .IsUnique();

            entity.Property(e => e.Points)
                .IsRequired();
        });
    }
}
