namespace GoodOnYouScrapper.Service.Utils;

/// <summary>Class representing constants used in the application</summary>
public abstract class AppConstants
{
    /// <summary>Api name</summary>
    public const string ApiName = "GoodOnYou Scrapper Web API (.NET 7)"; 
    
    /// <summary>Api description</summary>
    public const string ApiDescription = "This API allows you to get information about a brand from GoodOnYou website.";

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

    /// <summary>Base URL to access GoodOnYou</summary>
    public const string WebSiteUrl = "https://directory.goodonyou.eco/brand/";

    /// <summary>XPath for the environment rating in the brand page</summary>
    public const string XPathEnvironmentRating = "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[1]/div/div[2]/div/span";

    
    /// <summary>XPath for the people rating in the brand page</summary>
    public const string XPathPeopleRating = "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[2]/div/div[2]/div/span";

    
    /// <summary>XPath for the animal rating in the brand page</summary>
    public const string XPathAnimalRating = "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[3]/div/div[2]/div/span";
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[3]/div/div[2]/div/span";

    /// <summary>XPath for the rating description in the brand page</summary>
    public const string XPathRatingDescription = "//*[@id='rating-summary-text']";
        "//*[@id='rating-summary-text']";

    /// <summary>Regex to extract the rating from the rating description</summary>
    public const string XPathBrandCountry = "//*[@id='brand-hero-summary']/div/div[2]/p[3]";
        "//*[@id='brand-hero-summary']/div/div[2]/p[3]";

    /// <summary>XPath for sidebar</summary>
    public const string XPathSideBar = "//*[@id='sideBar']";
        "//*[@id='sideBar']";
    
}