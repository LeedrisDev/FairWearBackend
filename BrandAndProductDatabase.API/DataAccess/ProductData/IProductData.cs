using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.DataAccess.ProductData;

/// <summary>Interface that defines the methods for ProductData.</summary>
public interface IProductData
{
    /// <summary>
    /// Call the appropriate microservice to get a product by its name.
    /// </summary>
    /// <param name="barcode">Name of the product to get.</param>
    /// <returns> A <see cref="ProcessingStatusResponse{T}"/> containing the product. If the product exists.</returns>
    Task<ProcessingStatusResponse<ProductDto>> GetProductByBarcode(string barcode);
}