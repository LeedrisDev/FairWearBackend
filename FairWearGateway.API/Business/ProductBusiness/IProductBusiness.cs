using BrandAndProductDatabase.API.Models.Response;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.Business.ProductBusiness;


/// <summary>Interface that handles the business logic for the Product model.</summary>
public interface IProductBusiness
{
    /// <summary>Call the data access to get all the products in the database.</summary>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> that contains the list of product which are in database
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync();
    
    /// <summary>Call the data access to get a product by its id.</summary>
    /// <param name="productId">Id of the wanted product</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the product object if the call succeed
    /// </returns>
    Task<ProcessingStatusResponse<ProductResponse>> GetProductByIdAsync(int productId);
}