using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<LocationReading> LocationReadings { get; set; }
    public DbSet<VitalSignReading> VitalSignReadings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Expedition>().HasKey(i => i.Id);
        builder.Entity<Expedition>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();

        // Incident Configuration
        builder.Entity<Incident>().HasKey(i => i.Id);
        builder.Entity<Incident>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Incident>().Property(i => i.ExpeditionId).IsRequired();
        builder.Entity<Incident>().Property(i => i.ReportedBy).IsRequired();
        builder.Entity<Incident>().Property(i => i.Description).IsRequired().HasMaxLength(500);
        builder.Entity<Incident>().Property(i => i.Severity).IsRequired().HasMaxLength(50);
        builder.Entity<Incident>().Property(i => i.Status).IsRequired().HasMaxLength(50);
        builder.Entity<Incident>().Property(i => i.ReportedAt).IsRequired().HasMaxLength(100);

        // Alert Configuration
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.ExpeditionId).IsRequired();
        builder.Entity<Alert>().Property(a => a.TouristId).IsRequired();
        builder.Entity<Alert>().Property(a => a.Type).IsRequired();
        builder.Entity<Alert>().Property(a => a.Severity).IsRequired();
        builder.Entity<Alert>().Property(a => a.Status).IsRequired();
        builder.Entity<Alert>().Property(a => a.Message).IsRequired().HasMaxLength(500);

        // LocationReading Configuration
        builder.Entity<LocationReading>().HasKey(l => l.Id);
        builder.Entity<LocationReading>().Property(l => l.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<LocationReading>().Property(l => l.ExpeditionId).IsRequired();
        builder.Entity<LocationReading>().Property(l => l.TouristId).IsRequired();
        builder.Entity<LocationReading>().Property(l => l.Latitude).IsRequired();
        builder.Entity<LocationReading>().Property(l => l.Longitude).IsRequired();
        builder.Entity<LocationReading>().Property(l => l.AccuracyMeters).IsRequired();
        builder.Entity<LocationReading>().Property(l => l.RecordedAt).IsRequired();

        // VitalSignReading Configuration
        builder.Entity<VitalSignReading>().HasKey(v => v.Id);
        builder.Entity<VitalSignReading>().Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<VitalSignReading>().Property(v => v.ExpeditionId).IsRequired();
        builder.Entity<VitalSignReading>().Property(v => v.TouristId).IsRequired();
        builder.Entity<VitalSignReading>().Property(v => v.HeartRate).IsRequired();
        builder.Entity<VitalSignReading>().Property(v => v.BloodOxygen).IsRequired();
        builder.Entity<VitalSignReading>().Property(v => v.BodyTemperature).IsRequired();
        builder.Entity<VitalSignReading>().Property(v => v.RecordedAt).IsRequired();

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
