using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.DataAccess.IRepositories;

/// <summary>Interface for BrandRepository</summary>
public interface IBrandRepository : IRepository<Models.Dto.BrandDto>
{
    /// <summary>Gets a single brand by its name.</summary>
    /// <param name="name">The name of the brand to get.</param>
    /// <returns>A single brand.</returns>
    Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name);
}