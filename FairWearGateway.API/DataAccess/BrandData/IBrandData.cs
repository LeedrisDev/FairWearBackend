using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.DataAccess.BrandData;

/// <summary>Interface that defines the methods to call the appropriate microservice to get all is related to brands.</summary>
public interface IBrandData
{
    /// <summary>Call the appropriate microservice to get a brand by its id.</summary>
    /// <param name="brandId">Id of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed
    /// </returns>
    ProcessingStatusResponse<BrandResponse> GetBrandById(int brandId);

    /// <summary>Call the appropriate microservice to get a brand by its name.</summary>
    /// <param name="name">Name of the wanted brand</param>
    /// <returns>
    /// A <see cref="ProcessingStatusResponse{T}"/> instance, that contains the brand object if the call succeed.
    /// </returns>
    ProcessingStatusResponse<BrandResponse> GetBrandByName(string name);
}