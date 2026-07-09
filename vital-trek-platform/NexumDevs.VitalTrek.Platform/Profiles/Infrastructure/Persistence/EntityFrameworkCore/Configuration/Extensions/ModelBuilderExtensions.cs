using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the Profiles bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<TouristProfile>(entity =>
        {
            entity.ToTable("TouristProfiles");

            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.UserId).IsUnique();

            entity.Property(p => p.FullName).IsRequired().HasMaxLength(200);
            entity.Property(p => p.PhotoUrl).HasMaxLength(500);
            entity.Property(p => p.Nationality).HasMaxLength(100);
            entity.Property(p => p.PhoneNumber).HasMaxLength(50);
            entity.Property(p => p.PreferredLanguage).HasMaxLength(10);
            entity.Property(p => p.ExperienceLevel).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.IdentityDocumentType).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.IdentityDocumentNumber).HasMaxLength(100);
            entity.Property(p => p.BloodType).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.Allergies).HasMaxLength(1000);
            entity.Property(p => p.MedicalConditions).HasMaxLength(1000);
            entity.Property(p => p.Medications).HasMaxLength(1000);

            entity.HasMany(p => p.EmergencyContacts)
                .WithOne()
                .HasForeignKey(c => c.TouristProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Metadata
                .FindNavigation(nameof(TouristProfile.EmergencyContacts))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<EmergencyContact>(entity =>
        {
            entity.ToTable("EmergencyContacts");

            entity.HasKey(c => c.Id);
            // Client-generated Guid, added into an already-tracked TouristProfile's
            // EmergencyContacts collection — without this, EF's default Guid-key convention can
            // emit an UPDATE instead of an INSERT for a brand-new contact (same bug found and
            // fixed for Support's TicketReply.Id and TourManagement's Checkpoint/TourAssignment.Id).
            entity.Property(c => c.Id).ValueGeneratedNever();
            entity.HasIndex(c => c.TouristProfileId);

            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Relationship).HasMaxLength(100);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(50);
        });

        builder.Entity<TouristPreferences>(entity =>
        {
            entity.ToTable("TouristPreferences");

            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.UserId).IsUnique();

            entity.Property(p => p.PreferredDifficulty).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.PreferredActivityTypesCsv).HasColumnName("PreferredActivityTypes").HasMaxLength(500);
            entity.Property(p => p.DietaryRestrictionsCsv).HasColumnName("DietaryRestrictions").HasMaxLength(500);

            // Computed, read-only projections of the Csv columns above — not persisted themselves.
            entity.Ignore(p => p.PreferredActivityTypes);
            entity.Ignore(p => p.DietaryRestrictions);
        });

        builder.Entity<StaffProfile>(entity =>
        {
            entity.ToTable("StaffProfiles");

            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.UserId).IsUnique();
            entity.HasIndex(p => p.AgencyId);

            entity.Property(p => p.FullName).IsRequired().HasMaxLength(200);
            entity.Property(p => p.PhotoUrl).HasMaxLength(500);
            entity.Property(p => p.Position).HasMaxLength(100);
            entity.Property(p => p.ContactPhone).HasMaxLength(50);
        });

        builder.Entity<StaffPreferences>(entity =>
        {
            entity.ToTable("StaffPreferences");

            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.UserId).IsUnique();
        });

        builder.Entity<MedicalDataAccessLog>(entity =>
        {
            entity.ToTable("MedicalDataAccessLogs");

            entity.HasKey(l => l.Id);
            entity.HasIndex(l => l.TouristProfileId);
        });
    }
}
