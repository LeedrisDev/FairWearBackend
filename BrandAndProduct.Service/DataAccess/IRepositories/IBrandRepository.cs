using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;

namespace BrandAndProduct.Service.DataAccess.IRepositories;

/// <summary>Interface for BrandRepository</summary>
public interface IBrandRepository : IRepository<BrandDto>
{
    /// <summary>Gets a single brand by its name.</summary>
    /// <param name="name">The name of the brand to get.</param>
    /// <returns>A single brand.</returns>
    Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name);
}