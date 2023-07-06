using FairWearProductDataRetriever.Service.Utils;
using FairWearProductDataRetriever.Service.Utils.HttpClientWrapper;
using HtmlAgilityPack;

namespace FairWearProductDataRetriever.Service.DataAccess.ProductData;

/// <summary>Data access for retrieving product information</summary>
public class ProductData : IProductData
{
    private readonly HtmlDocument _htmlDocument;
    private readonly IHttpClientWrapper _httpClient;

    /// <summary>Constructor</summary>
    /// <param name="httpClient">The <see cref="IHttpClientWrapper"/> to use</param>
    /// <param name="htmlDocument">The <see cref="HtmlDocument"/> to use</param>
    public ProductData(IHttpClientWrapper httpClient, HtmlDocument htmlDocument)
    {
        _httpClient = httpClient;
        _htmlDocument = htmlDocument;
    }

    /// <inheritdoc/>
    public async Task<HtmlDocument> GetBarcodeInfoPageHtml(string barCode)
    {
        var productPage = await GetBarcodeInfoPage(barCode);
        _htmlDocument.LoadHtml(productPage);
        return _htmlDocument;
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
}