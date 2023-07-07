using BrandAndProductDatabase.Service.Models;
using FairWearProductDataRetriever.Service.Protos;

namespace BrandAndProductDatabase.Service.DataAccess.ProductData;

/// <summary>Interface that defines the methods for ProductData.</summary>
public interface IProductData
{
    /// <summary>
    /// Call the appropriate microservice to get a product by its barcode.
    /// </summary>
    /// <param name="upc">barcode of the product</param>
    /// <returns> A <see cref="ProcessingStatusResponse{T}"/> containing the product. If the product exists.</returns>
    ProcessingStatusResponse<ProductScrapperResponse> GetProductByUpc(string upc);
}