using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;

namespace BrandAndProduct.Service.Business.BrandBusiness;

/// <summary>
/// Interface for BrandBusiness.
/// </summary>
public interface IBrandBusiness
{
    /// <summary>Gets all the Brands.</summary>
    /// <returns>A list of <see cref="BrandDto"/>.</returns>
    Task<ProcessingStatusResponse<IEnumerable<BrandDto>>> GetAllBrandsAsync(Dictionary<string, string> filters);

    /// <summary>Gets a Brand by Id.</summary>
    /// <param name="id">The Id of the Brand.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<ProcessingStatusResponse<BrandDto>> GetBrandByIdAsync(int id);

    /// <summary>Gets a Brand by Name.</summary>
    /// <param name="name">The Name of the Brand.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name);

    /// <summary>Creates a Brand.</summary>
    /// <param name="brandDto">The Brand to create.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<ProcessingStatusResponse<BrandDto>> CreateBrandAsync(BrandDto brandDto);

    /// <summary>Updates a Brand.</summary>
    /// <param name="brandDto">The Brand to update.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<ProcessingStatusResponse<BrandDto>> UpdateBrandAsync(BrandDto brandDto);

    /// <summary>Deletes a Brand.</summary>
    /// <param name="id">The Id of the Brand to delete.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task<ProcessingStatusResponse<BrandDto>> DeleteBrandAsync(int id);
}