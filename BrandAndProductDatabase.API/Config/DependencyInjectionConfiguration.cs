using BrandAndProductDatabase.API.Business.BrandBusiness;
using BrandAndProductDatabase.API.Business.ProductBusiness;
using BrandAndProductDatabase.API.DataAccess;
using BrandAndProductDatabase.API.DataAccess.BrandData;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.DataAccess.ProductData;
using BrandAndProductDatabase.API.DataAccess.Repositories;
using BrandAndProductDatabase.API.Utils;
using BrandAndProductDatabase.API.Utils.HttpClientWrapper;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.Config;


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
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IBrandData, BrandData>();
        services.AddTransient<IProductData, ProductData>();
        
        // Business
        services.AddTransient<IBrandBusiness, BrandBusiness>();
        services.AddTransient<IProductBusiness, ProductBusiness>();

        
    }
}