using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;

namespace FairWearGateway.API.DataAccess.ProductData;

/// <summary>Interface that defines the methods to call the appropriate microservice to get all is related to products.</summary>
public interface IProductData
{
    /// <summary>Call the appropriate microservice to get a product by its id.</summary>
    /// <param name="productId">Id of the wanted product</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product object if the call succeed
    /// </returns>
    Task<ProcessingStatusResponse<ProductResponse>> GetProductByIdAsync(int productId);

    /// <summary>
    /// Call the appropriate microservice to get a product by its Barcode (UPC)
    /// </summary>
    /// <param name="upc">The barcode</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product information object if the call succeed
    /// </returns>
    Task<ProcessingStatusResponse<ProductInformationResponse>> GetProductByUpcAsync(string upc);
}