
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context for the Learning Center Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <summary>
    ///     On creating the database model
    /// </summary>
    /// <remarks>
    ///     This method is used to create the database model for the application.
    /// </remarks>
    /// <param name="builder">
    ///     The model builder for the database context
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Expedition>().HasKey(i => i.Id);
        builder.Entity<Expedition>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Entity<Expedition>()
            .Property(i => i.TourID).IsRequired();
        builder.Entity<Expedition>()
            .Property(i => i.GuideID).IsRequired();
        builder.Entity<Expedition>()
            .Property(i => i.ExpeditionName).IsRequired();
        builder.Entity<Expedition>()
            .Property(i => i.Status).IsRequired();
        builder.UseSnakeCaseNamingConvention();
        
        builder.Entity<Experience>().HasKey(i => i.Id);
        builder.Entity<Experience>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<Experience>()
            .Property(i => i.ExpeditionID).IsRequired();
        builder.Entity<Experience>()
            .Property(i => i.TouristID).IsRequired();
        builder.Entity<Experience>()
            .Property(i => i.Note).IsRequired();
        builder.Entity<Experience>()
            .Property(i => i.MediaUrl).IsRequired();
        builder.UseSnakeCaseNamingConvention();
        
        builder.Entity<Progress>().HasKey(i => i.Id);
        builder.Entity<Progress>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<Progress>()
            .Property(i => i.ExpeditionId).IsRequired();
        builder.Entity<Progress>()
            .Property(i => i.CompletedCheckpoints).IsRequired();
        builder.Entity<Progress>()
            .Property(i => i.TotalCheckpoints).IsRequired();
        builder.Entity<Progress>()
            .Property(i => i.Percentage).IsRequired();
        builder.UseSnakeCaseNamingConvention();
        
        builder.Entity<Weather>().HasKey(i => i.Id);
        builder.Entity<Weather>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<Weather>()
            .Property(i => i.ExpeditionId).IsRequired();
        builder.Entity<Weather>()
            .Property(i => i.TemperatureCelsius).IsRequired();
        builder.Entity<Weather>()
            .Property(i => i.Condition).IsRequired();
        builder.Entity<Weather>()
            .Property(i => i.Humidity).IsRequired();
        builder.Entity<Weather>()
            .Property(i => i.WindSpeedKmh).IsRequired();
        builder.UseSnakeCaseNamingConvention();
    }
    
    public DbSet<BinnacleReading> BinnacleReadings { get; set; }
}
