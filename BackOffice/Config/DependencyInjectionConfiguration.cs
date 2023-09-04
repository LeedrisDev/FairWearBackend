using BackOffice.DataAccess;
using BackOffice.Utils;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Config;

/// <summary> Static class to configure dependency injection.</summary>
public abstract class DependencyInjectionConfiguration
{
    /// <summary>Configures dependency injection.</summary>
    public static void Configure(IServiceCollection services)
    {
        services.AddDbContext<BrandAndProductDbContext>(options =>
            options.UseNpgsql(AppConstants.Database.ConnectionString));
    }
}