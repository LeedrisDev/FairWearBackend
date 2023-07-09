using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.Business.BrandBusiness;

/// <summary>Interface that handles the business logic for the Brand model.</summary>
public interface IBrandBusiness
{
    /// <summary>Call the data access to get a brand by its id.</summary>
    /// <param name="brandId">Id of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed
    /// </returns>
    ProcessingStatusResponse<BrandResponse> GetBrandById(int brandId);

    /// <summary>Call the data access to get a brand by its name.</summary>
    /// <param name="brandName">Name of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed.
    /// </returns>
    ProcessingStatusResponse<BrandResponse> GetBrandByName(string brandName);

    /// <summary>Call the data access to get all brands.</summary>
    /// <param name="filters">filters to apply to list of brands</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the list of brand object if the call succeed.
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<BrandResponse>>> GetAllBrand(Dictionary<string, string> filters);
}