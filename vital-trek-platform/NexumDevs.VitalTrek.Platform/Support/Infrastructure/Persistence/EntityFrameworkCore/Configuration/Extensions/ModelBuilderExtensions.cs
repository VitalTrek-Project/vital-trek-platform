using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Support.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Support.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the Support bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies all Entity Framework Core configurations
    /// related to the Support bounded context.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ModelBuilder"/> used to configure entity mappings.
    /// </param>
    public static void ApplySupportConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Tickets");

            entity.HasKey(t => t.Id);

            entity.HasIndex(t => t.UserId);

            entity.Property(t => t.UserMode).IsRequired().HasMaxLength(50);
            entity.Property(t => t.FullName).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Email).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Subject).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Category).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(2000);
            entity.Property(t => t.Priority).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Status).IsRequired().HasMaxLength(50);

            entity.HasMany(t => t.Replies)
                .WithOne()
                .HasForeignKey(r => r.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Metadata
                .FindNavigation(nameof(Ticket.Replies))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<TicketReply>(entity =>
        {
            entity.ToTable("TicketReplies");

            entity.HasKey(r => r.Id);

            entity.HasIndex(r => r.TicketId);

            entity.Property(r => r.AuthorName).IsRequired().HasMaxLength(200);
            entity.Property(r => r.AuthorMode).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Message).IsRequired().HasMaxLength(2000);
        });
    }
}
