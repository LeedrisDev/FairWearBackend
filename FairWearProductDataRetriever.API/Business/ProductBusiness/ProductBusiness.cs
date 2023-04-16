using System.Net;
using System.Web;
using FairWearProductDataRetriever.API.DataAccess.ProductData;
using FairWearProductDataRetriever.API.Models;
using FairWearProductDataRetriever.API.Utils.AppConstants;
using HtmlAgilityPack;

namespace FairWearProductDataRetriever.API.Business.ProductBusiness;

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
            if (CheckForNotFound(htmlDocument))
            {
                throw new HttpRequestException("Product not found", null, HttpStatusCode.NotFound);
            }
        }
        catch (HttpRequestException e)
        {
            _processingStatusResponse.Status = e.StatusCode ?? HttpStatusCode.InternalServerError;
            _processingStatusResponse.ErrorMessage = e.Message;
            return _processingStatusResponse;
        }

        var productModel = new ProductModel()
        {
            UpcCode = barcode,
            Name = GetProductName(htmlDocument),
            BrandName = GetProductBrandName(htmlDocument),
            Category = GetProductCategory(htmlDocument),
        };
        
        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = productModel;

        return _processingStatusResponse;
    }

    private static string GetProductBrandName(HtmlDocument doc)
    { 
        var nodes = doc.DocumentNode
            .SelectSingleNode(AppConstants.XPathInformationTable)
            .ChildNodes
            .Where(node => node.Name == "tr").ToList();

        var brandNode =
            nodes.Find(
            node => node.ChildNodes.Any( child => child.Name == "td" && child.FirstChild.InnerText == "Brand")
            )?.ChildNodes.Where(node => node.Name == "td").ToList();


        var str = HttpUtility.HtmlDecode(brandNode?[1].InnerText ?? "");
        return str;
    }

    private static string GetProductName(HtmlDocument doc)
    {
        var node = doc.DocumentNode
            .SelectSingleNode(AppConstants.XPathProductName);

        return node.InnerText;
    }

    private static string GetProductCategory(HtmlDocument doc)
    {
        var nodes = doc.DocumentNode
            .SelectSingleNode(AppConstants.XPathInformationTable)
            .ChildNodes
            .Where(node => node.Name == "tr").ToList();

        var categoryNodes =
            nodes.Find(
                node => node.ChildNodes.Any(child => child.Name == "td" && child.FirstChild.InnerText == "Category")
            )?.ChildNodes.Where(node => node.Name == "td").ToList();


        var str = HttpUtility.HtmlDecode(categoryNodes?[1].InnerText ?? "");
        return str;
    }
    
    private static bool CheckForNotFound(HtmlDocument doc)
    {
        try
        {

            var node = doc.DocumentNode.SelectSingleNode("//*[@id='resultPageContainer']/p");
    
            if (node != null)
            {
                var text = node.InnerText;
                if (text.Contains("Sorry, we were not able to find a product for"))
                {
                    return true;
                }
            }
    
            return false;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }
}