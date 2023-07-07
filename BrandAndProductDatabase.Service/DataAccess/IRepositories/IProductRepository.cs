using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;

namespace BrandAndProductDatabase.Service.DataAccess.IRepositories;

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