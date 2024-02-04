using BackOffice.Business.Classes;
using BackOffice.Business.Interfaces;
using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using BackOffice.DataAccess.Repositories.Classes;
using BackOffice.DataAccess.Repositories.Interfaces;
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

        services.AddDefaultIdentity<UserEntity>(options => { options.SignIn.RequireConfirmedAccount = false; })
            .AddEntityFrameworkStores<AuthenticationDbContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = new PathString("/Account/Login");
            options.LogoutPath = new PathString("/Account/Logout");
            options.Cookie.Name = "BackOfficeCookie";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(2);
        });

        services.AddAutoMapper(typeof(AutoMapperProfiles));

        // Repositories
        services.AddTransient<IBrandRepository, BrandRepository>();

        // Business 
        services.AddTransient<IBrandBusiness, BrandBusiness>();
    }
}