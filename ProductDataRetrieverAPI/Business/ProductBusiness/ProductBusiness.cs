using System.Net;
using HtmlAgilityPack;
using ProductDataRetrieverAPI.DataAccess.ProductData;
using ProductDataRetrieverAPI.Models;
using ProductDataRetrieverAPI.Utils.AppConstants;

namespace ProductDataRetrieverAPI.Business.ProductBusiness;

public class ProductBusiness : IProductBusiness
{
    private readonly ProcessingStatusResponse<ProductModel> _processingStatusResponse;
    private readonly IProductData _productData;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="processingStatusResponse"></param>
    /// <param name="productData"></param>
    public ProductBusiness(IProductData productData)
    {
        _processingStatusResponse = new ProcessingStatusResponse<ProductModel>();
        _productData = productData;
    }

    /// <summary>
    /// Retrieves information for a product
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    public async Task<ProcessingStatusResponse<ProductModel>> GetProductInformation(string barcode)
    {
        HtmlDocument htmlDocument;

        try
        {
            htmlDocument = await _productData.GetBarcodeInfoPageHtml(barcode);
        }
        catch (HttpRequestException e)
        {
            _processingStatusResponse.Status = e.StatusCode ?? HttpStatusCode.InternalServerError;
            _processingStatusResponse.ErrorMessage = e.Message;
            return _processingStatusResponse;
        }

        var productModel = new ProductModel()
        {
            BrandName = GetBrandName(htmlDocument),
        };
        
        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = productModel;

        return _processingStatusResponse;
    }

    private static string GetBrandName(HtmlDocument doc)
    { 
        var nodes = doc.DocumentNode
            .SelectSingleNode("//*[@id='resultPageContainer']/div/div[1]/div[1]/table")
            .ChildNodes
            .Where(node => node.Name == "tr").ToList();

        var brandNode =
            nodes.Find(
            node => node.ChildNodes.Any( child => child.Name == "td" && child.FirstChild.InnerText == "Brand")
            )?.ChildNodes.Where(node => node.Name == "td").ToList();
        
        return brandNode?[1].InnerText ?? "";
    }
    
}