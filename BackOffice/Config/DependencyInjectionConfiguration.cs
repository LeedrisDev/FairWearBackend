using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
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
            options.UseNpgsql(AppConstants.Database.BrandAndProductConnectionString));
        // TODO: Pass this connection string as an environment variable.
        services.AddDbContext<AuthenticationDbContext>(options =>
            options.UseNpgsql("User ID=sa;Password=sa;Host=localhost;Port=5433;Database=authentication_db;"));
        
        services.AddDefaultIdentity<UserEntity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AuthenticationDbContext>();
    }
}