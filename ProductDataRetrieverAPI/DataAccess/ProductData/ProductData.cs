using HtmlAgilityPack;
using ProductDataRetrieverAPI.Utils.HttpClientWrapper;
using ProductDataRetrieverAPI.Utils.AppConstants;

namespace ProductDataRetrieverAPI.DataAccess.ProductData;

public class ProductData : IProductData
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly HtmlDocument _htmlDocument;
    
    /// <summary>Constructor</summary>
    /// <param name="httpClient">The <see cref="IHttpClientWrapper"/> to use</param>
    /// <param name="htmlDocument">The <see cref="HtmlDocument"/> to use</param>
    public ProductData(IHttpClientWrapper httpClient, HtmlDocument htmlDocument)
    {
        _httpClient = httpClient;
        _htmlDocument = htmlDocument;
    }
    
    private async Task<string> GetBarcodeInfoPage(string barcode)
    {
        var response = await _httpClient.GetAsync(AppConstants.WebSiteUrl + barcode);
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HtmlDocument> GetBarcodeInfoPageHtml(string barCode)
    {
        var productPage = await GetBarcodeInfoPage(barCode);
        _htmlDocument.LoadHtml(productPage);
        return _htmlDocument;
    }
    
}