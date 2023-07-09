using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.Business.ProductBusiness;

/// <summary>Interface that handles the business logic for the Product model.</summary>
public interface IProductBusiness
{
    /// <summary>Call the data access to get a product by its id.</summary>
    /// <param name="productId">Id of the wanted product</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product object if the call succeed
    /// </returns>
    ProcessingStatusResponse<ProductResponse> GetProductById(int productId);

    /// <summary>Call the data access to get a product information by its barcode.</summary>
    /// <param name="upc">barcode of the product</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product object if the call succeed
    /// </returns>
    ProcessingStatusResponse<ProductInformationResponse> GetProductByUpc(string upc);

    /// <summary>Call the data access to get all products.</summary>
    /// <param name="filters">filters to apply to list of products</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the list of product object if the call succeed.
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<ProductResponse>>> GetAllProducts(Dictionary<string, string> filters);
}