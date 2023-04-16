using System.Net;
using HtmlAgilityPack;
using ProductDataRetrieverAPI.DataAccess.ProductData;
using ProductDataRetrieverAPI.Models;

namespace ProductDataRetrieverAPI.Business.ProductBusiness;

public class ProductBusiness: IProductBusiness
{
    private readonly ProcessingStatusResponse<ProductModel> _processingStatusResponse;
    private readonly IProductData _productData;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="processingStatusResponse"></param>
    /// <param name="productData"></param>
    public ProductBusiness(ProcessingStatusResponse<ProductModel> processingStatusResponse, IProductData productData)
    {
        _processingStatusResponse = processingStatusResponse;
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
            _processingStatusResponse.Status = e.StatusCode?? HttpStatusCode.InternalServerError;
            _processingStatusResponse.ErrorMessage = e.Message;
            return _processingStatusResponse;
        }

        var productModel = new ProductModel();
        
        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = productModel;

        return _processingStatusResponse;
    }
}