using FairWearGateway.API.Models;
using FairWearGateway.API.Models.ServiceResponse;

namespace FairWearGateway.API.DataAccess.BrandData;

public interface IBrandData
{
    /// <summary>Gets all the brands in the database</summary>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> that contains the list of brand which are in database
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<BrandServiceResponse>>> GetAllBrandsAsync();

    /// <summary>Get a brand from it's Id</summary>
    /// <param name="brandId">Id of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed
    /// </returns>
    Task<ProcessingStatusResponse<BrandServiceResponse>> GetBrandByIdAsync(int brandId);
}