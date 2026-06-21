using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        // Monitoring Context
    

        builder.Entity<Incident>().HasKey(t => t.Id);
        builder.Entity<Incident>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Incident>().Property(t => t.ExpeditionId).IsRequired();
        builder.Entity<Incident>().Property(t => t.ReportedBy).IsRequired();
        builder.Entity<Incident>().Property(t => t.Description).IsRequired();
        builder.Entity<Incident>().Property(t => t.Status).IsRequired();
        builder.Entity<Incident>().Property(t => t.Severity).IsRequired();
        builder.Entity<Incident>().Property(t => t.ReportedAt).IsRequired();

      
    }
}