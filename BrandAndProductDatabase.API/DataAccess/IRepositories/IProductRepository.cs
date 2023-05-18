using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.DataAccess.IRepositories;

/// <summary>Interface for ProductRepository</summary>
public interface IProductRepository : IRepository<Models.Dto.ProductDto>
{
    /// <summary>Get a product by barcode.</summary>
    /// <param name="barcode"> The barcode of the product to get.</param>
    /// <returns>
    /// <see cref="ProcessingStatusResponse{TModel}"/> containing the product with the given barcode. if exists.
    /// </returns>
    public Task<ProcessingStatusResponse<ProductDto>> GetProductByBarcodeAsync(string barcode);
}