using FairWearProductDataRetriever.API.Utils.AppConstants;
using FairWearProductDataRetriever.API.Utils.HttpClientWrapper;
using HtmlAgilityPack;

namespace FairWearProductDataRetriever.API.DataAccess.ProductData;

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
    
    /// <summary>
    /// Retrieves the product page
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    private async Task<string> GetBarcodeInfoPage(string barcode)
    {
        var response = await _httpClient.GetAsync(AppConstants.WebSiteUrl + barcode);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    /// <inheritdoc/>
    public async Task<HtmlDocument> GetBarcodeInfoPageHtml(string barCode)
    {
        var productPage = await GetBarcodeInfoPage(barCode);
        _htmlDocument.LoadHtml(productPage);
        return _htmlDocument;
    }

}