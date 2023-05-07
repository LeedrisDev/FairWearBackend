using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.Business.BrandBusiness;

/// <summary>
/// Interface for BrandBusiness.
/// </summary>
public interface IBrandBusiness
{
    /// <summary>Gets all the Brands.</summary>
    /// <returns>A list of <see cref="BrandDto"/>.</returns>
    Task<IEnumerable<BrandDto>> GetAllBrandsAsync();

    /// <summary>Gets a Brand by Id.</summary>
    /// <param name="id">The Id of the Brand.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<BrandDto> GetBrandByIdAsync(int id);

    /// <summary>Creates a Brand.</summary>
    /// <param name="brandDto">The Brand to create.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<BrandDto> CreateBrandAsync(BrandDto brandDto);

    /// <summary>Updates a Brand.</summary>
    /// <param name="brandDto">The Brand to update.</param>
    /// <returns>A <see cref="BrandDto"/>.</returns>
    Task<BrandDto> UpdateBrandAsync(BrandDto brandDto);

    /// <summary>Deletes a Brand.</summary>
    /// <param name="id">The Id of the Brand to delete.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task DeleteBrandAsync(int id);
}