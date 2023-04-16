using ProductDataRetrieverAPI.Models;

namespace ProductDataRetrieverAPI.Business.ProductBusiness;

public interface IProductBusiness
{
    public Task<ProcessingStatusResponse<ProductModel>> GetProductInformation(string barcode);
}