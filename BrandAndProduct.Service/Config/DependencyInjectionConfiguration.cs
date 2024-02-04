using BrandAndProduct.Service.Business.BrandBusiness;
using BrandAndProduct.Service.Business.IntegrationEventBusiness;
using BrandAndProduct.Service.Business.ProductBusiness;
using BrandAndProduct.Service.DataAccess;
using BrandAndProduct.Service.DataAccess.BrandData;
using BrandAndProduct.Service.DataAccess.Filters;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.DataAccess.ProductData;
using BrandAndProduct.Service.DataAccess.Repositories;
using BrandAndProduct.Service.Services;
using BrandAndProduct.Service.Utils;
using BrandAndProduct.Service.Utils.HttpClientWrapper;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProduct.Service.Config;

/// <summary>Static class that configures the dependency injection.</summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>Configures the dependency injection.</summary>
    public static void Configure(IServiceCollection services)
    {
        // DbContext
        services.AddDbContext<BrandAndProductDbContext>(options =>
        {
            options.UseNpgsql(AppConstants.Database.BrandAndProductConnectionString);
        });

        // Services
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
        services.AddAutoMapper(typeof(AutoMapperProfiles));

        // DataAccess
        services.AddTransient<IBrandRepository, BrandRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IBrandData, BrandData>();
        services.AddTransient<IProductData, ProductData>();
        services.AddTransient<IIntegrationEventRepository, IntegrationEventRepository>();
        services.AddTransient<IFilterFactory<IFilter>, GenericFilterFactory<IFilter>>();

        // Business
        services.AddTransient<IBrandBusiness, BrandBusiness>();
        services.AddTransient<IProductBusiness, ProductBusiness>();
        services.AddTransient<IIntegrationEventBusiness, IntegrationEventBusiness>();

        services.AddSingleton<IIntegrationEventSenderService, IntegrationEventSenderService>();

        services.AddHostedService<IntegrationEventSenderService>(provider =>
        {
            // Resolve IntegrationEventSenderService from the service provider
            var integrationEventSenderService = provider.GetRequiredService<IIntegrationEventSenderService>();

            // Return the resolved instance as the hosted service
            return (IntegrationEventSenderService)integrationEventSenderService;
        });
    }
}