using HtmlAgilityPack;

namespace ProductDataRetriever.Service.DataAccess.ProductData;

/// <summary>Interface for the product data access.</summary>
public interface IProductData
{
    /// <summary>Get the product page</summary>
    /// <param name="barcode">barcode number</param>
    /// <returns>An <see cref="HtmlDocument"/> containing all information on the product</returns>
    public Task<HtmlDocument> GetBarcodeInfoPageHtml(string barcode);
}