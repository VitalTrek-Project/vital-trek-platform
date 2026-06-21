using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace NexumDevs.VitalTrek.Platform;

/// <summary>
/// Design-time factory used by Entity Framework Core tools to create
/// instances of <see cref="AppDbContext"/> during migrations and other
/// design-time operations.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates and configures an instance of <see cref="AppDbContext"/>
    /// using the connection string defined in the application configuration files.
    /// </summary>
    /// <param name="args">
    /// Command-line arguments provided by Entity Framework Core tools.
    /// </param>
    /// <returns>
    /// A configured instance of <see cref="AppDbContext"/>.
    /// </returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySQL(connectionString!);

        return new AppDbContext(optionsBuilder.Options);
    }
}