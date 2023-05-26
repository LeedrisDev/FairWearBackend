using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;

namespace FairWearGateway.API.Business.BrandBusiness;

/// <summary>Interface that handles the business logic for the Brand model.</summary>
public interface IBrandBusiness
{
    /// <summary>Call the data access to get all the brands in the database.</summary>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> that contains the list of brand which are in database
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<BrandResponse>>> GetAllBrandsAsync();
    
    /// <summary>Call the data access to get a brand by its id.</summary>
    /// <param name="brandId">Id of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed
    /// </returns>
    Task<ProcessingStatusResponse<BrandResponse>> GetBrandByIdAsync(int brandId);
    
    /// <summary>Call the data access to get a brand by its name.</summary>
    /// <param name="brandName">Name of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed.
    /// </returns>
    Task<ProcessingStatusResponse<BrandResponse>> GetBrandByNameAsync(string brandName);
}