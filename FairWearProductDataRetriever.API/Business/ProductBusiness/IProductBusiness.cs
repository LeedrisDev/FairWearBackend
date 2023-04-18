using FairWearProductDataRetriever.API.Models;

namespace FairWearProductDataRetriever.API.Business.ProductBusiness;

public interface IProductBusiness
{
    /// <summary>Retrieves information for a product.</summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    public Task<ProcessingStatusResponse<ProductModel>> GetProductInformation(string barcode);
}