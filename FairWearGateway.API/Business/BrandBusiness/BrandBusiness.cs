using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.DataAccess.BrandData;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.Business.BrandBusiness;

/// <summary>Class that handles the business logic for the Brand model.</summary>
public class BrandBusiness : IBrandBusiness
{
    private readonly IBrandData _brandData;

    /// <summary>Initializes a new instance of the <see cref="BrandBusiness"/> class.</summary>
    public BrandBusiness(IBrandData brandData)
    {
        _brandData = brandData;
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<BrandResponse> GetBrandById(int brandId)
    {
        return _brandData.GetBrandById(brandId);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<BrandResponse> GetBrandByName(string brandName)
    {
        return _brandData.GetBrandByName(brandName);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<BrandResponse>>> GetAllBrands(
        Dictionary<string, string> filters)
    {
        return await _brandData.GetAllBrands(filters);
    }
}