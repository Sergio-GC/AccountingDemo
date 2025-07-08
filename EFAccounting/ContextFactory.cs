using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EFAccounting;
using Microsoft.Extensions.Configuration;

public class ContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        // Build configuration to access appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // or use AppContext.BaseDirectory for flexibility
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("DefaultDb");

        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();

        return new Context(optionsBuilder.Options);
    }
}
