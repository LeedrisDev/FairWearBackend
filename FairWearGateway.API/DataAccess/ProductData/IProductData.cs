using BrandAndProduct.Service.Protos;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.DataAccess.ProductData;

/// <summary>Interface that defines the methods to call the appropriate microservice to get all is related to products.</summary>
public interface IProductData
{
    /// <summary>Call the appropriate microservice to get a product by its id.</summary>
    /// <param name="productId">Id of the wanted product</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product object if the call succeed
    /// </returns>
    ProcessingStatusResponse<ProductResponse> GetProductById(int productId);

    /// <summary>
    /// Call the appropriate microservice to get a product by its Barcode (UPC)
    /// </summary>
    /// <param name="upc">The barcode</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product information object if the call succeed
    /// </returns>
    ProcessingStatusResponse<ProductInformationResponse> GetProductByUpc(string upc);

    /// <summary>Call the appropriate microservice to get all products.</summary>
    /// <param name="filters">filters to apply to list of products</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the list of product object if the call succeed.
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<ProductResponse>>> GetAllProducts(Dictionary<string, string> filters);
}