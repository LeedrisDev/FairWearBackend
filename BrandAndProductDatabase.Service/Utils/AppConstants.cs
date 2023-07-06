namespace BrandAndProductDatabase.Service.Utils;

/// <summary>Class to hold constants for the application.</summary>
public static class AppConstants
{
    private static readonly string GoodOnYouScrapperServiceHost =
        Environment.GetEnvironmentVariable("GOODONYOU_SCRAPPER_SERVICE_HOST")!;

    private static readonly string GoodOnYouScrapperServicePortHttp =
        Environment.GetEnvironmentVariable("GOODONYOU_SCRAPPER_SERVICE_PORT_HTTP")!;

    private static readonly string ProductDataRetrieverServiceHost =
        Environment.GetEnvironmentVariable("PRODUCT_DATA_RETRIEVER_SERVICE_HOST")!;

    private static readonly string ProductDataRetrieverServicePortHttp =
        Environment.GetEnvironmentVariable("PRODUCT_DATA_RETRIEVER_SERVICE_PORT_HTTP")!;

    /// <summary>URL for the Good On You Scrapper microservice.</summary>
    public static readonly string GoodOnYouScrapperUrl =
        $"http://{GoodOnYouScrapperServiceHost}:{GoodOnYouScrapperServicePortHttp}/api/brand";

    /// <summary>URL for the Product Data Retriever microservice.</summary>
    public static readonly string ProductDataRetrieverUrl =
        $"http://{ProductDataRetrieverServiceHost}:{ProductDataRetrieverServicePortHttp}/api/product";

    /// <summary>Host of the Brand and Product Database.</summary>
    public static readonly string BrandAndProductDatabaseHost =
        Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_SERVICE_HOST")!;

    /// <summary>Port of the Brand and Product Database.</summary>
    public static readonly string BrandAndProductDatabasePort =
        Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_SERVICE_PORT_HTTP")!;
}