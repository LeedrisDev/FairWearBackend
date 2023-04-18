namespace FairWearProductDataRetriever.API.Utils;

/// <summary>Contains all constants used in the API.</summary>
public abstract class AppConstants
{
    /// <summary>Api name</summary>
    public const string ApiName = "Product Data Retriever API (.NET 7)"; 
    
    /// <summary>Api description</summary>
    public const string ApiDescription = "This API is used to retrieve product information from a barcode.";

    /// <summary>FairWear mail</summary>
    public const string FairWearMail = "fairwear.group@gmail.com";
    
    // API Versions
    /// <summary>API major version</summary>
    public const int ApiMajorVersion = 1;
    /// <summary>API minor version</summary>
    public const int ApiMinorVersion = 0;
    
    /// <summary>Description if version is deprecated</summary>
    public const string ApiVersionDeprecatedDescription = "This API version has been deprecated. " +
                                                          "Please use one of the new APIs available from the explorer.";

    /// <summary>Base URL to access go-upc</summary>
    public static readonly string WebSiteUrl = "https://go-upc.com/search?q=";
    
    /// <summary> XPath to the information table containing EAN, Brand and Category </summary>
    public static readonly string XPathInformationTable =
        "//*[@id='resultPageContainer']/div/div[1]/div[1]/table";
    
    /// <summary> XPath to the product name </summary>
    public static readonly string XPathProductName =
        "//*[@id='resultPageContainer']/div/div[1]/div[1]/h1";
    
    /// <summary> XPath to the message NotFound </summary>
    public static readonly string XPathNotFound =
        "//*[@id='resultPageContainer']/p";




}