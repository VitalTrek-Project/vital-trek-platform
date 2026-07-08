using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Iam.Domain.Model.Aggregates;

namespace NexumDevs.VitalTrek.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the Iam bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies all Entity Framework Core configurations
    /// related to the Iam bounded context.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ModelBuilder"/> used to configure entity mappings.
    /// </param>
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Username).IsUnique();

            entity.Property(u => u.PasswordHash).IsRequired();

            entity.Property(u => u.Role).IsRequired().HasConversion<string>().HasMaxLength(20);

            entity.HasIndex(u => u.AgencyId);
        });
    }
}
