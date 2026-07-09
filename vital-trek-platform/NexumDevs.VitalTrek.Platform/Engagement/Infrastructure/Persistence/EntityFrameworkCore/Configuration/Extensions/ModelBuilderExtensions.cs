using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the Engagement (Loyalty) bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    // Fixed ids for globally-seeded badge definitions — must stay stable across migrations.
    private static readonly Guid FirstExpeditionBadgeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid FiveExpeditionsBadgeId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid FirstReviewBadgeId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid FirstReferralBadgeId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private static readonly DateTimeOffset SeedTimestamp = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public static void ApplyEngagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<GamificationProfile>(entity =>
        {
            entity.ToTable("GamificationProfiles");
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => new { p.TouristId, p.AgencyId }).IsUnique();
            entity.Property(p => p.TotalPoints).IsRequired();
            // Optimistic concurrency: two concurrent redemptions/point-events for the same
            // profile both read the same TotalPoints, both pass their balance check, and both
            // would otherwise commit — overspending the balance. Marking the raced-on column
            // itself as the concurrency token makes the second SaveChanges affect 0 rows and
            // throw DbUpdateConcurrencyException instead of silently overspending.
            entity.Property(p => p.TotalPoints).IsConcurrencyToken();
        });

        builder.Entity<PointsTransaction>(entity =>
        {
            entity.ToTable("PointsTransactions");
            entity.HasKey(t => t.Id);
            // No .HasFilter(): MySQL has no partial/filtered index support. This relies
            // instead on standard SQL semantics — NULL is never equal to NULL in a unique
            // index, so any number of rows with a NULL SourceId are still allowed; only
            // non-null (TouristId, AgencyId, Type, SourceId) combinations are deduplicated.
            entity.HasIndex(t => new { t.TouristId, t.AgencyId, t.Type, t.SourceId })
                .IsUnique();
            entity.HasIndex(t => t.ProfileId);
            entity.Property(t => t.Points).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(500).IsRequired();
            entity.Property(t => t.SourceId).HasMaxLength(100);
        });

        builder.Entity<LoyaltyProgram>(entity =>
        {
            entity.ToTable("LoyaltyPrograms");
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.AgencyId).IsUnique();
        });

        builder.Entity<LoyaltyTier>(entity =>
        {
            entity.ToTable("LoyaltyTiers");
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.AgencyId);
            entity.Property(t => t.Name).HasMaxLength(100).IsRequired();
            entity.Property(t => t.Benefits).HasMaxLength(1000);
        });

        builder.Entity<BadgeDefinition>(entity =>
        {
            entity.ToTable("BadgeDefinitions");
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.Code).IsUnique();
            entity.Property(b => b.Code).HasMaxLength(100).IsRequired();
            entity.Property(b => b.Name).HasMaxLength(200).IsRequired();
            entity.Property(b => b.Description).HasMaxLength(500);

            entity.HasData(
                new
                {
                    Id = FirstExpeditionBadgeId,
                    AgencyId = (Guid?)null,
                    Code = "first_expedition",
                    Name = "Primera expedición",
                    Description = "Completaste tu primera expedición.",
                    RuleType = BadgeRuleType.ExpeditionCount,
                    RuleThreshold = 1,
                    CreatedAt = SeedTimestamp
                },
                new
                {
                    Id = FiveExpeditionsBadgeId,
                    AgencyId = (Guid?)null,
                    Code = "five_expeditions",
                    Name = "5 expediciones",
                    Description = "Completaste cinco expediciones.",
                    RuleType = BadgeRuleType.ExpeditionCount,
                    RuleThreshold = 5,
                    CreatedAt = SeedTimestamp
                },
                new
                {
                    Id = FirstReviewBadgeId,
                    AgencyId = (Guid?)null,
                    Code = "first_review",
                    Name = "Primera reseña",
                    Description = "Dejaste tu primera reseña.",
                    RuleType = BadgeRuleType.ReviewCount,
                    RuleThreshold = 1,
                    CreatedAt = SeedTimestamp
                },
                new
                {
                    Id = FirstReferralBadgeId,
                    AgencyId = (Guid?)null,
                    Code = "first_referral",
                    Name = "Primer referido",
                    Description = "Un turista que referiste completó su primera expedición.",
                    RuleType = BadgeRuleType.ReferralCount,
                    RuleThreshold = 1,
                    CreatedAt = SeedTimestamp
                });
        });

        builder.Entity<AwardedBadge>(entity =>
        {
            entity.ToTable("AwardedBadges");
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => new { b.TouristId, b.AgencyId, b.BadgeDefinitionId }).IsUnique();
        });

        builder.Entity<Reward>(entity =>
        {
            entity.ToTable("Rewards");
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => r.AgencyId);
            entity.Property(r => r.Name).HasMaxLength(200).IsRequired();
            entity.Property(r => r.Description).HasMaxLength(1000);
            // Optimistic concurrency on Stock: see the same comment on GamificationProfile's
            // TotalPoints above — prevents overselling limited stock under concurrent redemptions.
            entity.Property(r => r.Stock).IsConcurrencyToken();
        });

        builder.Entity<Redemption>(entity =>
        {
            entity.ToTable("Redemptions");
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => r.Code).IsUnique();
            entity.HasIndex(r => new { r.AgencyId, r.TouristId });
            entity.Property(r => r.Code).HasMaxLength(20).IsRequired();
        });

        builder.Entity<ReferralCode>(entity =>
        {
            entity.ToTable("ReferralCodes");
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Code).IsUnique();
            entity.HasIndex(c => new { c.TouristId, c.AgencyId }).IsUnique();
            entity.Property(c => c.Code).HasMaxLength(20).IsRequired();
        });

        builder.Entity<Referral>(entity =>
        {
            entity.ToTable("Referrals");
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => new { r.AgencyId, r.ReferredTouristId });
        });

        builder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => new { r.AgencyId, r.TouristId, r.ExpeditionId }).IsUnique();
            entity.Property(r => r.Comment).HasMaxLength(2000);
        });

        builder.Entity<InAppNotification>(entity =>
        {
            entity.ToTable("InAppNotifications");
            entity.HasKey(n => n.Id);
            entity.HasIndex(n => new { n.RecipientTouristId, n.IsRead });
            entity.Property(n => n.Title).HasMaxLength(200).IsRequired();
            entity.Property(n => n.Message).HasMaxLength(1000).IsRequired();
        });
    }
}
