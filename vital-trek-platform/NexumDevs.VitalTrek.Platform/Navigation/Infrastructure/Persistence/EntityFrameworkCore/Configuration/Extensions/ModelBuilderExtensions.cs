using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyNavigationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Expedition>().HasKey(t => t.Id);
        builder.Entity<Expedition>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Expedition>().Property(t => t.TourID).IsRequired();
        builder.Entity<Expedition>().Property(t => t.GuideID).IsRequired();
        builder.Entity<Expedition>().Property(t => t.ExpeditionName).IsRequired();
        builder.Entity<Expedition>().Property(t => t.Status).IsRequired();
        
        builder.Entity<Experience>().HasKey(t => t.Id);
        builder.Entity<Experience>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Experience>().Property(t => t.ExpeditionID).IsRequired();
        builder.Entity<Experience>().Property(t => t.TouristID).IsRequired();
        builder.Entity<Experience>().Property(t => t.Note).IsRequired();
        builder.Entity<Experience>().Property(t => t.MediaUrl).IsRequired();
        
        builder.Entity<Progress>().HasKey(t => t.Id);
        builder.Entity<Progress>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Progress>().Property(t => t.CompletedCheckpoints).IsRequired();
        builder.Entity<Progress>().Property(t => t.TotalCheckpoints).IsRequired();
        builder.Entity<Progress>().Property(t => t.Percentage).IsRequired();
        
        builder.Entity<Weather>().HasKey(t => t.Id);
        builder.Entity<Weather>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Weather>().Property(t => t.TemperatureCelsius).IsRequired();
        builder.Entity<Weather>().Property(t => t.Condition).IsRequired();
        builder.Entity<Weather>().Property(t => t.Humidity).IsRequired();
        builder.Entity<Weather>().Property(t => t.WindSpeedKmh).IsRequired();
        
        builder.Entity<BinnacleReading>();
    }
}
