using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;

namespace BrandAndProduct.Service.DataAccess.IRepositories;

/// <summary>Interface for ProductRepository</summary>
public interface IProductRepository : IRepository<ProductDto>
{
    /// <summary>
    /// Update a product in database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<ProcessingStatusResponse<ProductDto>> UpdateProductAsync(ProductDto entity);
    
    /// <summary>
    /// Get recommended products for a product
    /// </summary>
    /// <param name="productId">The product id to get recommended products for.</param>
    /// <returns></returns>
    Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetRecommendedProductsAsync(int productId);
}