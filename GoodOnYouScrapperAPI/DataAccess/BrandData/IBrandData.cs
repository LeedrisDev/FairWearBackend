using HtmlAgilityPack;

namespace GoodOnYouScrapperAPI.DataAccess.BrandData;

/// <summary>Interface for the brand data access layer</summary>
public interface IBrandData
{
    /// <summary>Get the brand page</summary>
    /// <param name="brandName">Name of the brand</param>
    /// <returns>An <see cref="HtmlDocument"/> containing all information on the brand</returns>
    public Task<HtmlDocument> GetBrandPageHtml(string brandName);
}