namespace BrandAndProductDatabase.API.Utils;

/// <summary>Class to hold constants for the application.</summary>
public static class AppConstants
{
    /// <summary>URL for the Good On You Scrapper microservice.</summary>
    public const string GoodOnYouScrapperUrl = "http://good_on_you_scrapper_api:80/api/brand";

    /// <summary>URL for the Product Data Retriever microservice.</summary>
    public const string ProductDataRetrieverUrl = "http://product_data_retriever_api:80/api/product";
}