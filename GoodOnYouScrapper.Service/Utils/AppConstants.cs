namespace GoodOnYouScrapper.Service.Utils;

/// <summary>Class representing constants used in the application</summary>
public abstract class AppConstants
{
    /// <summary>XPath for the brand name in the brand page</summary>
    public const string XPathBrandName = "//*[@id='brand-hero-summary']/h1";

    /// <summary>Base URL to access GoodOnYou</summary>
    public const string WebSiteUrl = "https://directory.goodonyou.eco/brand/";

    /// <summary>XPath for the environment rating in the brand page</summary>
    public const string XPathEnvironmentRating = "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[1]/div/div[2]/div/span";

    /// <summary>XPath for the people rating in the brand page</summary>
    public const string XPathPeopleRating = "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[2]/div/div[2]/div/span";

    /// <summary>XPath for the animal rating in the brand page</summary>
    public const string XPathAnimalRating = "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[3]/div/div[2]/div/span";

    /// <summary>XPath for the rating description in the brand page</summary>
    public const string XPathRatingDescription = "//*[@id='rating-summary-text']";

    /// <summary>Regex to extract the rating from the rating description</summary>
    public const string XPathBrandCountry = "//*[@id='brand-hero-summary']/div/div[2]/p[3]";

    /// <summary>XPath for sidebar</summary>
    public const string XPathSideBar = "//*[@id='sideBar']";
}