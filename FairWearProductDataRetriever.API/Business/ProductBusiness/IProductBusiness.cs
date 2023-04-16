using FairWearProductDataRetriever.API.Models;

namespace FairWearProductDataRetriever.API.Business.ProductBusiness;

public interface IProductBusiness
{
    public Task<ProcessingStatusResponse<ProductModel>> GetProductInformation(string barcode);
}