namespace GoodOnYouScrapperAPI.Utils.AppConstants;

public class AppConstants: IAppConstants
{
    public static readonly string WebSiteUrl = "https://directory.goodonyou.eco/brand/";

    public static readonly string XPathEnvironmentRating =
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[1]/div/div[2]/div/span";
    
    public static readonly string XPathPeopleRating =
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[2]/div/div[2]/div/span";
    
    public static readonly string XPathAnimalRating =
        "//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[3]/div/div[2]/div/span";
    
}