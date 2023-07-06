using FairWearProductDataRetriever.Service.Business.ProductBusiness;
using FairWearProductDataRetriever.Service.DataAccess.ProductData;
using FairWearProductDataRetriever.Service.Utils.HttpClientWrapper;
using HtmlAgilityPack;

namespace FairWearProductDataRetriever.Service.Config;

/// <summary>Static class that configures the dependency injection.</summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>Configures the dependency injection.</summary>
    public static void Configure(IServiceCollection services)
    {
        // Services
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
        services.AddTransient<HtmlDocument>();

        // DataAccess
        services.AddTransient<IProductData, ProductData>();

        // Business
        services.AddTransient<IProductBusiness, ProductBusiness>();
    }
}