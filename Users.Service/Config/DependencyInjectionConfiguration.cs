using Microsoft.EntityFrameworkCore;
using Users.Service.Business.ProductBusiness;
using Users.Service.Business.UserBusiness;
using Users.Service.Business.UserExperienceBusiness;
using Users.Service.Business.UserProductHistoryBusiness;
using Users.Service.DataAccess;
using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.DataAccess.Repositories;
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

        // Services
        services.AddAutoMapper(typeof(AutoMapperProfiles));

        // DataAccess
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserExperienceRepository, UserExperienceRepository>();
        services.AddTransient<IUserProductHistoryRepository, UserProductHistoryRepository>();

        services.AddTransient<IFilterFactory<IFilter>, GenericFilterFactory<IFilter>>();

        // Business
        services.AddTransient<IProductBusiness, ProductBusiness>();
        services.AddTransient<IUserBusiness, UserBusiness>();
        services.AddTransient<IUserExperienceBusiness, UserExperienceBusiness>();
        services.AddTransient<IUserProductHistoryBusiness, UserProductHistoryBusiness>();
    }
}