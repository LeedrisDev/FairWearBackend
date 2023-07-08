using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;

namespace BrandAndProductDatabase.Service.DataAccess.BrandData;

/// <summary>Interface that defines the methods for BrandData.</summary>
public interface IBrandData
{
    /// <summary>Call the appropriate microservice to get a brand by its name.</summary>
    /// <param name="name"> Name of the brand to get.</param>
    /// <returns>A <see cref="ProcessingStatusResponse{T}"/> containing the brand. If the brand exists.</returns>
    ProcessingStatusResponse<BrandDto> GetBrandByName(string name);
}