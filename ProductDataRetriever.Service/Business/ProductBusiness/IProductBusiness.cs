using ProductDataRetriever.Service.Protos;
using ProductDataRetriever.Service.Models;

namespace ProductDataRetriever.Service.Business.ProductBusiness;

/// <summary>Interface for the product business.</summary>
public interface IProductBusiness
{
    /// <summary>Retrieves information for a product.</summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    public Task<ProcessingStatusResponse<ProductScrapperResponse>> GetProductInformation(string barcode);
}