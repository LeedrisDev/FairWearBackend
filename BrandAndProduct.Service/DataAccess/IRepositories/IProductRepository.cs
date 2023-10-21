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
}