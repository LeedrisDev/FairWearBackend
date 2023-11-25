using Microsoft.EntityFrameworkCore;
using Users.Service.DataAccess;
using Users.Service.Utils;

namespace Users.Service.Config;

/// <summary>Static class that configures the dependency injection.</summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>Configures the dependency injection.</summary>
    public static void Configure(IServiceCollection services)
    {
        // DbContext
        services.AddDbContext<UsersDbContext>(options =>
        {
            options.UseNpgsql(AppConstants.Database.BrandAndProductConnectionString);
        });

        services.AddAutoMapper(typeof(AutoMapperProfiles));
    }
}