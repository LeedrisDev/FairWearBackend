using FairWearGateway.API.Business.BrandBusiness;
using FairWearGateway.API.Business.ProductBusiness;
using FairWearGateway.API.Business.UserBusiness;
using FairWearGateway.API.Business.UserExperienceBusiness;
using FairWearGateway.API.Business.UserProductHistoryBusiness;
using FairWearGateway.API.DataAccess.BrandData;
using FairWearGateway.API.DataAccess.ProductData;
using FairWearGateway.API.DataAccess.UserData;
using FairWearGateway.API.DataAccess.UserExperienceData;
using FairWearGateway.API.DataAccess.UserProductHistoryData;

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
        services.AddTransient<IUserData, UserData>();
        services.AddTransient<IUserExperienceData, UserExperienceData>();
        services.AddTransient<IUserProductHistoryData, UserProductHistoryData>();

        // Business
        services.AddTransient<IProductBusiness, ProductBusiness>();
        services.AddTransient<IBrandBusiness, BrandBusiness>();
        services.AddTransient<IUserBusiness, UserBusiness>();
        services.AddTransient<IUserExperienceBusiness, UserExperienceBusiness>();
        services.AddTransient<IUserProductHistoryBusiness, UserProductHistoryBusiness>();
    }
}