namespace FairWearGateway.API.Utils;

/// <summary>Class that contains the constants of the application.</summary>
public static class AppConstants
{
    private static readonly string BrandAndProductApiServiceHost =
        Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_API_SERVICE_HOST")!;

    private static readonly string BrandAndProductApiServicePortHttp =
        Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_API_SERVICE_PORT_HTTP")!;

    private static readonly string UserServiceHost =
        Environment.GetEnvironmentVariable("USERS_API_SERVICE_HOST")!;

    private static readonly string UserServicePortHttp =
        Environment.GetEnvironmentVariable("USERS_API_SERVICE_PORT_HTTP")!;

    /// <summary>URL of the brand and product service.</summary>
    public static readonly string BrandAndProductServiceUrl =
        $"http://{BrandAndProductApiServiceHost}:{BrandAndProductApiServicePortHttp}";

    /// <summary>URL of the user service.</summary>
    public static readonly string UserServiceUrl =
        $"http://{UserServiceHost}:{UserServicePortHttp}";
}