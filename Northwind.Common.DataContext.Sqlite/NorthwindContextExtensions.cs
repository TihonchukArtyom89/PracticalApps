using Microsoft.EntityFrameworkCore;//UseSqlite
using Microsoft.Extensions.DependencyInjection;//ISeviceCollection

namespace Packt.Shared;
public static class NorthwindContextExtensions
{
    /// <summary>
    /// Add Northwind Context to the specified IServiceCollection. Uses the Sqlite database provider
    /// </summary>
    /// <param name="services"></param>
    /// <param name="relativePath">Set to override th default of ".." </param>
    /// <returns>An IserviceCollection that can be used to add more services.</returns>
    public static IServiceCollection AddNorthwindContext(this IServiceCollection services,string relativePath="..")
    {
        string databasePath = Path.Combine(relativePath, "Northwind.db");
        services.AddDbContext<NorthwindContext>(options =>
        {
            options.UseSqlite($"Data Source={databasePath}");
            options.LogTo(WriteLine,//Console
                  new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        });
        return services;
    }
}
