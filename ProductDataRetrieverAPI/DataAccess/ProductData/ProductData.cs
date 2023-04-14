using GoodOnYouScrapperAPI.Utils.HttpClientWrapper;
using HtmlAgilityPack;
using ProductDataRetrieverAPI.Utils.AppConstants;

namespace ProductDataRetrieverAPI.DataAccess.ProductData;

public class ProductData
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly HtmlDocument _htmlDocument;
    private async Task<string> GetBarcodeInfoPage(string barCode)
    {
        var response = await _httpClient.GetAsync(AppConstants.WebSiteUrl + barCode);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HtmlDocument> GetBarcodeInfoPageHtml(string barCode)
    {
        var brandPage = await GetBarcodeInfoPage(barCode);
        _htmlDocument.LoadHtml(brandPage);
        return _htmlDocument;
    }
}