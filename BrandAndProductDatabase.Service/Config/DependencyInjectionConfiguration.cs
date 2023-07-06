using BrandAndProductDatabase.Service.Business.BrandBusiness;
using BrandAndProductDatabase.Service.DataAccess;
using BrandAndProductDatabase.Service.DataAccess.BrandData;
using BrandAndProductDatabase.Service.DataAccess.IRepositories;
using BrandAndProductDatabase.Service.DataAccess.Repositories;
using BrandAndProductDatabase.Service.Utils;
using BrandAndProductDatabase.Service.Utils.HttpClientWrapper;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.Service.Config;

/// <summary>Static class that configures the dependency injection.</summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>Configures the dependency injection.</summary>
    public static void Configure(IServiceCollection services)
    {
        // DbContext
        services.AddDbContext<BrandAndProductDbContext>(options =>
        {
            options.UseNpgsql(
                $"User ID=fairwear;Password=fairwear;Host={AppConstants.BrandAndProductDatabaseHost};Port={AppConstants.BrandAndProductDatabasePort};Database=fairwear_brand_and_product_database;");
        });

        // Services
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
        services.AddAutoMapper(typeof(AutoMapperProfiles));

        // DataAccess
        services.AddTransient<IBrandRepository, BrandRepository>();
        // services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IBrandData, BrandData>();
        // services.AddTransient<IProductData, ProductData>();

        // Business
        services.AddTransient<IBrandBusiness, BrandBusiness>();
        // services.AddTransient<IProductBusiness, ProductBusiness>();
    }
}