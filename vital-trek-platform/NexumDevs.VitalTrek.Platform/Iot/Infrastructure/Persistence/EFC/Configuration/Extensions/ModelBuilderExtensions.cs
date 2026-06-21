using Microsoft.EntityFrameworkCore;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Iot.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
/// Provides Entity Framework Core Fluent API configurations
/// for the IoT bounded context entities.
/// This extension is invoked from <c>AppDbContext.OnModelCreating</c>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies all Entity Framework Core configurations
    /// related to the IoT bounded context.
    /// </summary>
    public static void ApplyIoTConfiguration(this ModelBuilder builder)
    {
        builder.Entity<IoTDevice>(entity =>
        {
            entity.ToTable("IotDevices");

            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();

            entity.Property(d => d.Name).IsRequired().HasMaxLength(150);

            entity.Property(d => d.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(d => d.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(d => d.LastSeen);
            entity.Property(d => d.LastCommand).HasMaxLength(100);

            // Cross-BC references by ID — no EF FK navigation properties
            entity.Property(d => d.ExpeditionId);
            entity.Property(d => d.TouristId);
        });

        builder.Entity<SensorReading>(entity =>
        {
            entity.ToTable("SensorReadings");

            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();

            entity.Property(r => r.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            entity.Property(r => r.Value).IsRequired();
            entity.Property(r => r.Unit).HasMaxLength(30);
            entity.Property(r => r.RecordedAt).IsRequired();

            // Intra-BC FK: SensorReading → IoTDevice (same bounded context)
            entity.HasOne<IoTDevice>()
                .WithMany()
                .HasForeignKey(r => r.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}