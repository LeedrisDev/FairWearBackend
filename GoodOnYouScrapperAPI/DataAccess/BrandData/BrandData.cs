using GoodOnYouScrapperAPI.Utils.AppConstants;
using GoodOnYouScrapperAPI.Utils.HttpClientWrapper;
using HtmlAgilityPack;

namespace GoodOnYouScrapperAPI.DataAccess.BrandData;

/// <summary>Get the brand page from GoodOnYou site</summary>
public class BrandData: IBrandData
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly HtmlDocument _htmlDocument;
    
    /// <summary>Constructor</summary>
    /// <param name="httpClient">The <see cref="IHttpClientWrapper"/> to use</param>
    /// <param name="htmlDocument">The <see cref="HtmlDocument"/> to use</param>
    public BrandData(IHttpClientWrapper httpClient, HtmlDocument htmlDocument)
    {
        _httpClient = httpClient;
        _htmlDocument = htmlDocument;
    }
    
    /// <inheritdoc />
    public async Task<HtmlDocument> GetBrandPageHtml(string brandName)
    {
        var brandPage = await GetBrandPage(brandName);
        _htmlDocument.LoadHtml(brandPage);
        return _htmlDocument;
    }
    
    private async Task<string> GetBrandPage(string brandName)
    {
        var response = await _httpClient.GetAsync(AppConstants.WebSiteUrl + brandName);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}