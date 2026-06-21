using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.TourManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the TourManagement bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies all Entity Framework Core configurations
    /// related to the TourManagement bounded context.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ModelBuilder"/> used to configure entity mappings.
    /// </param>
    public static void ApplyTourManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Tour>(entity =>
        {
            entity.ToTable("Tours");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Description)
                .HasMaxLength(1000);

            entity.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.Difficulty)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.DistanceKm)
                .HasColumnType("decimal(8,2)");

            entity.HasMany(t => t.Checkpoints)
                .WithOne()
                .HasForeignKey(c => c.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(t => t.Assignments)
                .WithOne()
                .HasForeignKey(a => a.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Metadata
                .FindNavigation(nameof(Tour.Checkpoints))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata
                .FindNavigation(nameof(Tour.Assignments))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Ignore(t => t.DomainEvents);
        });
        
        builder.Entity<Checkpoint>(entity =>
        {
            entity.ToTable("TourCheckpoints");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Latitude)
                .IsRequired();

            entity.Property(c => c.Longitude)
                .IsRequired();
        });
        
        builder.Entity<TourAssignment>(entity =>
        {
            entity.ToTable("TourAssignments");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasIndex(a => new { a.TourId, a.TouristId })
                .IsUnique();
        });
    }
}