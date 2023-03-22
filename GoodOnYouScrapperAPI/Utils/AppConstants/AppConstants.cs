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
}