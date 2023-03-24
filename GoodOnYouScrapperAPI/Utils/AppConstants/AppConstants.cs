namespace GoodOnYouScrapperAPI.Utils.AppConstants;

public abstract class AppConstants: IAppConstants
{
    /// <summary>Api name</summary>
    public const string ApiName = "GoodOnYou Scrapper Web API (.NET 7)"; 
    
    // API Versions
    /// <summary>API major version</summary>
    public const int ApiMajorVersion = 1;
    /// <summary>API minor version</summary>
    public const int ApiMinorVersion = 0;
    
    /// <summary>Description if version is deprecated</summary>
    public const string ApiVersionDeprecatedDescription = "This API version has been deprecated. " +
                                                          "Please use one of the new APIs available from the explorer.";

    /// <summary>Base URL to access GoodOnYou</summary>
    public static readonly string WebSiteUrl = "https://directory.goodonyou.eco/brand/";

    public static readonly string XPathEnvironmentRating =
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[1]/div/div[2]/div/span";
    
    public static readonly string XPathPeopleRating =
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[2]/div/div[2]/div/span";
    
    public static readonly string XPathAnimalRating =
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[3]/div/div[2]/div/span";

    public static readonly string XPathRatingDescription =
        "//*[@id='rating-summary-text']";
}