namespace FairWearProductDataRetriever.API.Utils;

/// <summary>Contains all constants used in the API.</summary>
public abstract class AppConstants
{
    /// <summary>Api name</summary>
    public const string ApiName = "Product Data Retriever API (.NET 7)";

    /// <summary>Description if version is deprecated</summary>
    public const string ApiVersionDeprecatedDescription = "This API version has been deprecated. " +
                                                          "Please use one of the new APIs available from the explorer.";

    /// <summary>Base URL to access go-upc</summary>
    public const string WebSiteUrl = "https://go-upc.com/search?q=";
    
    /// <summary> XPath to the information table containing EAN, Brand and Category </summary>
    public const string PathInformationTable =
        "//*[@id='resultPageContainer']/div/div[1]/div[1]/table";
    
    /// <summary> XPath to the product name </summary>
    public const string PathProductName =
        "//*[@id='resultPageContainer']/div/div[1]/div[1]/h1";
    
    /// <summary> XPath to the message NotFound </summary>
    public const string PathNotFound =
        "//*[@id='resultPageContainer']/p";




}