using HtmlAgilityPack;

namespace FairWearProductDataRetriever.API.DataAccess.ProductData;

public interface IProductData
{
    /// <summary>Get the product page</summary>
    /// <param name="barcode">barcode number</param>
    /// <returns>An <see cref="HtmlDocument"/> containing all information on the product</returns>
    public Task<HtmlDocument> GetBarcodeInfoPageHtml(string barcode);
}