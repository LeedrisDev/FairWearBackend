using System.Net;
using System.Web;
using FairWearProductDataRetriever.Service.DataAccess.ProductData;
using FairWearProductDataRetriever.Service.Models;
using FairWearProductDataRetriever.Service.Protos;
using FairWearProductDataRetriever.Service.Utils;
using HtmlAgilityPack;

namespace FairWearProductDataRetriever.Service.Business.ProductBusiness;

/// <summary>Business logic for retrieving product information.</summary>
public class ProductBusiness : IProductBusiness
{
    private readonly ProcessingStatusResponse<ProductScrapperResponse> _processingStatusResponse;
    private readonly IProductData _productData;

    /// <summary>/// Constructor</summary>
    /// <param name="productData">DataAccess of products</param>
    public ProductBusiness(IProductData productData)
    {
        _processingStatusResponse = new ProcessingStatusResponse<ProductScrapperResponse>();
        _productData = productData;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductScrapperResponse>> GetProductInformation(string barcode)
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

        var productResponse = new ProductScrapperResponse()
        {
            UpcCode = barcode,
            Name = GetProductName(htmlDocument),
            BrandName = GetProductBrandName(htmlDocument),
            Category = GetProductCategory(htmlDocument),
        };

        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = productResponse;

        return _processingStatusResponse;
    }

    /// <summary>Retrieves product brand name from the html document.</summary>
    /// <param name="doc">Html document of the product from go-upc</param>
    /// <returns></returns>
    private static string GetProductBrandName(HtmlDocument doc)
    {
        var nodes = doc.DocumentNode
            .SelectSingleNode(AppConstants.PathInformationTable)
            .ChildNodes
            .Where(node => node.Name == "tr").ToList();

        var brandNode =
            nodes.Find(
                node => node.ChildNodes.Any(child => child.Name == "td" && child.FirstChild.InnerText == "Brand")
            )?.ChildNodes.Where(node => node.Name == "td").ToList();


        var str = HttpUtility.HtmlDecode(brandNode?[1].InnerText ?? "");
        return str;
    }

    /// <summary>Retrieve product name from the html document.</summary>
    /// <param name="doc">Html document of the product from go-upc</param>
    /// <returns></returns>
    private static string GetProductName(HtmlDocument doc)
    {
        var node = doc.DocumentNode
            .SelectSingleNode(AppConstants.PathProductName);

        return node.InnerText;
    }


    /// <summary>Retrieves product category from the html document.</summary>
    /// <param name="doc">Html document of the product from go-upc</param>
    /// <returns></returns>
    private static string GetProductCategory(HtmlDocument doc)
    {
        var nodes = doc.DocumentNode
            .SelectSingleNode(AppConstants.PathInformationTable)
            .ChildNodes
            .Where(node => node.Name == "tr").ToList();

        var categoryNodes =
            nodes.Find(
                node => node.ChildNodes.Any(child => child.Name == "td" && child.FirstChild.InnerText == "Category")
            )?.ChildNodes.Where(node => node.Name == "td").ToList();


        var str = HttpUtility.HtmlDecode(categoryNodes?[1].InnerText ?? "");
        return str;
    }

    /// <summary>Checks if the product was not found.</summary>
    /// <param name="doc">Html document of the product from go-upc</param>
    /// <returns></returns>
    private static bool CheckForNotFound(HtmlDocument doc)
    {
        var node = doc.DocumentNode.SelectSingleNode(AppConstants.PathNotFound);

        if (node == null)
        {
            return false;
        }

        var text = node.InnerText;

        return text.Contains("Sorry, we were not able to find a product for");
    }
}