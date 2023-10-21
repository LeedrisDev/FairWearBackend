using FairWearGateway.API.Business.BrandBusiness;
using FairWearGateway.API.Business.ProductBusiness;
using FairWearGateway.API.DataAccess.BrandData;
using FairWearGateway.API.DataAccess.ProductData;

namespace FairWearGateway.API.Config;


/// <summary>Static class that configures the dependency injection.</summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>Configures the dependency injection.</summary>
    public static void Configure(IServiceCollection services)
    {
        // DataAccess
        services.AddTransient<IProductData, ProductData>();
        services.AddTransient<IBrandData, BrandData>();

        // Business
        services.AddTransient<IProductBusiness, ProductBusiness>();
        services.AddTransient<IBrandBusiness, BrandBusiness>();
    }
}